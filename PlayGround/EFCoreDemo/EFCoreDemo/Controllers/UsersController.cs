using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Data;
using EFCoreDemo.Models;

namespace EFCoreDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // ═══════════════════════════════════════════════════════════════
        // 🔥 핵심 8: DbContext 의존성 주입
        // ═══════════════════════════════════════════════════════════════
        // ❌ 기존: 생성자에서 new SqlConnection("...") 직접 생성 → 자원 관리 복잡
        // ✅ EF Core: DI로 자동 주입 → 수명(Lifetime) 자동 관리
        private readonly AppDbContext _db;

        // 생성자 주입: ASP.NET Core가 자동으로 AppDbContext instance 생성해서 넘겨줌
        public UsersController(AppDbContext db)
        {
            _db = db;
        }


        // ─────────────────────────────────────────
        // GET /api/users
        // 전체 유저 조회
        // ─────────────────────────────────────────
        
        // 🔥 핵심 9: READ (조회) - LINQ 쿼리
        // ─────────────────────────────────────────
        // ❌ 기존 CRUD:
        //   string sql = "SELECT * FROM Users";
        //   SqlCommand cmd = new SqlCommand(sql, connection);
        //   SqlDataReader reader = cmd.ExecuteReader();
        //   while (reader.Read()) {
        //       users.Add(new User { Id = reader.GetInt32(0), Name = reader.GetString(1), ... });
        //   }
        //   connection.Close();
        
        // ✅ EF Core: 한 줄로 끝남! + LINQ 지원 + 비동기 처리
        // - ToListAsync() = 비동기 실행 → UI 블로킹 안 함
        // - LINQ 쿼리 → 내부적으로 SQL 자동 생성 및 실행
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            // SELECT * FROM Users 자동 생성 및 실행!
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }


        // ─────────────────────────────────────────
        // GET /api/users/{id}
        // 특정 유저 조회
        // ─────────────────────────────────────────
        
        // 🔥 핵심 10: Primary Key로 검색 (최적화된 쿼리)
        // ─────────────────────────────────────────
        // ❌ 기존:
        //   SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Id = @id", conn);
        //   cmd.Parameters.AddWithValue("@id", id);
        //   SqlDataReader reader = cmd.ExecuteReader();
        //   if (reader.Read()) { ... }
        
        // ✅ EF Core의 FindAsync():
        // - PK 검색 최적화 → 내부 캐시(Identity Map) 활용
        // - 지난번과 같은 Id 검색 → DB 갔다오지 않고 메모리에서 가져옴 (성능 향상!)
        // - 매개변수 SQL Injection 자동 방지
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            // SELECT * FROM Users WHERE Id = @id 자동 생성!
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { message = $"ID {id}인 유저가 없어요" });

            return Ok(user);
        }


        // ─────────────────────────────────────────
        // POST /api/users
        // 새 유저 추가
        // ─────────────────────────────────────────
        
        // 🔥 핵심 11: CREATE (생성) - Change Tracking 자동 관리
        // ─────────────────────────────────────────
        // ❌ 기존:
        //   SqlCommand cmd = new SqlCommand(
        //       "INSERT INTO Users (Name, Email, Age) VALUES (@name, @email, @age)", conn);
        //   cmd.Parameters.AddWithValue("@name", user.Name);
        //   cmd.Parameters.AddWithValue("@email", user.Email);
        //   cmd.Parameters.AddWithValue("@age", user.Age);
        //   int inserted = cmd.ExecuteNonQuery();
        //   if (inserted > 0) { /* 성공 */ }
        
        // ✅ EF Core:
        // 1. _db.Users.Add() = 객체를 "추가 대기" 상태로 변경 (아직 DB 안 가함)
        // 2. SaveChangesAsync() = 실제로 INSERT SQL 생성 + 실행
        // 3. EF Core가 자동으로 Identity 값(자동증가 Id) 할당 + 객체에 반영
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            // 입력값 검증
            if (string.IsNullOrEmpty(user.Name))
                return BadRequest(new { message = "이름은 필수예요" });

            if (string.IsNullOrEmpty(user.Email) || !user.Email.Contains("@"))
                return BadRequest(new { message = "이메일 형식이 올바르지 않아요" });

            // ① DB에 추가 예약 (메모리에만 존재 - 아직 SQL 미실행)
            _db.Users.Add(user);

            // ② 실제로 DB에 저장
            // INSERT INTO Users (Name, Email, Age) VALUES (...) 자동 생성 및 실행!
            await _db.SaveChangesAsync();

            // ③ EF Core가 자동으로 DB에서 생성된 Id를 객체에 할당 (Identity 값)
            // class 프로퍼티 메타데이터에서 IDENTITY 칼럼 감지 후 값 매핑
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }


        // ─────────────────────────────────────────
        // PUT /api/users/{id}
        // 유저 수정
        // ─────────────────────────────────────────
        
        // 🔥 핵심 12: UPDATE (수정) - Change Tracking 자동 감지
        // ─────────────────────────────────────────
        // ❌ 기존:
        //   SqlCommand cmd = new SqlCommand(
        //       "UPDATE Users SET Name = @name, Email = @email, Age = @age WHERE Id = @id", conn);
        //   cmd.ExecuteNonQuery();
        
        // ✅ EF Core의 자동 Change Tracking:
        // - FindAsync()로 가져온 user 객체는 EF Core의 추적 상태 → 변경 감지 자동
        // - user.Name 변경 → EF Core가 변경사항을 메모리에 기록
        // - SaveChangesAsync() → 변경된 필드만 UPDATE SQL 생성 (최적화!)
        // → UPDATE Users SET Name = @name, Email = @email, Age = @age WHERE Id = @id
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] User updated)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { message = $"ID {id}인 유저가 없어요" });

            // ① 프로퍼티 변경 (메모리에서만 처리)
            // EF Core의 Change Tracker가 이 변경을 감지
            user.Name  = updated.Name;
            user.Email = updated.Email;
            user.Age   = updated.Age;

            // ② DB에 실제로 저장
            // UPDATE Users SET ... WHERE Id = ... 자동 생성 및 실행!
            await _db.SaveChangesAsync();

            return Ok(user);
        }


        // ─────────────────────────────────────────
        // DELETE /api/users/{id}
        // 유저 삭제
        // ─────────────────────────────────────────
        
        // 🔥 핵심 13: DELETE (삭제)
        // ─────────────────────────────────────────
        // ❌ 기존:
        //   SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Id = @id", conn);
        //   cmd.ExecuteNonQuery();
        
        // ✅ EF Core:
        // - Remove() = 객체를 "삭제 대기" 상태로 변경
        // - SaveChangesAsync() = DELETE SQL 생성 + 실행
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { message = $"ID {id}인 유저가 없어요" });

            // ① 삭제 예약
            _db.Users.Remove(user);

            // ② DB에 저장 (DELETE SQL 실행!)
            // DELETE FROM Users WHERE Id = ... 자동 생성 및 실행
            await _db.SaveChangesAsync();

            return Ok(new { message = $"{user.Name} 삭제 완료" });
        }


        // ─────────────────────────────────────────
        // GET /api/users/{id}/orders
        // 특정 유저의 주문 목록 조회
        // ─────────────────────────────────────────
        
        // 🔥 핵심 14: JOIN 쿼리 (관계 매핑의 힘!)
        // ─────────────────────────────────────────
        // ❌ 기존: 복잡한 JOIN SQL 직접 작성
        //   SELECT o.* FROM Orders o
        //   INNER JOIN Users u ON o.UserId = u.Id
        //   WHERE u.Id = @userId
        //   + SqlDataReader로 Order 객체 수동 매핑
        
        // ✅ EF Core의 LINQ:
        // - 객체 관계로 쿼리 작성 → SQL 자동 생성
        // - o => o.UserId == id → WHERE 조건 매핑
        // - 내부적으로 JOIN 자동 처리 (명시할 필요 없음)
        // - LINQ Where().ToListAsync() → SELECT WHERE 자동 생성
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<List<Order>>> GetOrders(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { message = $"ID {id}인 유저가 없어요" });

            // LINQ 쿼리 → SQL 자동 생성
            // SELECT o.* FROM Orders o WHERE o.UserId = @id
            var orders = await _db.Orders
                .Where(o => o.UserId == id)  // WHERE 조건
                .ToListAsync();               // 비동기 실행 + List로 변환

            return Ok(orders);
        }
    }
}