using Microsoft.AspNetCore.Mvc;
using PracticeApi.Models;

namespace PracticeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // 실제 DB 대신 메모리 리스트 사용 (static = 앱이 켜있는 동안 유지)
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "Sabin",    Email = "sabin@email.com",    Age = 35, Role = "admin" },
            new User { Id = 2, Name = "홍길동",   Email = "hong@email.com",     Age = 28, Role = "user"  },
            new User { Id = 3, Name = "김민준",   Email = "minjun@email.com",   Age = 25, Role = "user"  }
        };

        private readonly ILogger<UsersController> _logger;

        // 생성자: ILogger를 DI로 주입받음
        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }


        // ─────────────────────────────────────────
        // GET /api/users
        // 전체 유저 목록 조회
        // ─────────────────────────────────────────
        /// <summary>전체 유저 목록을 조회합니다</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public ActionResult<List<User>> GetAll()
        {
            _logger.LogInformation("유저 전체 목록 조회 — 총 {Count}명", _users.Count);
            return Ok(_users);
        }


        // ─────────────────────────────────────────
        // GET /api/users/{id}
        // 특정 유저 조회
        // ─────────────────────────────────────────
        /// <summary>ID로 특정 유저를 조회합니다</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                _logger.LogWarning("유저 없음: ID {Id}", id);
                return NotFound(new { message = $"ID {id}인 유저가 없어요" });
            }

            return Ok(user);
        }


        // ─────────────────────────────────────────
        // POST /api/users
        // 새 유저 추가
        // ─────────────────────────────────────────
        /// <summary>새 유저를 추가합니다</summary>
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Create([FromBody] User user)
        {
            // ★ 입력값 검증 (Input Validation)
            if (string.IsNullOrEmpty(user.Name))
                return BadRequest(new { message = "이름은 필수예요" });

            if (string.IsNullOrEmpty(user.Email))
                return BadRequest(new { message = "이메일은 필수예요" });

            if (!user.Email.Contains("@"))
                return BadRequest(new { message = "이메일 형식이 올바르지 않아요" });

            if (user.Age < 1 || user.Age > 120)
                return BadRequest(new { message = "나이는 1~120 사이여야 해요" });

            if (user.Role != "user" && user.Role != "admin")
                return BadRequest(new { message = "역할은 'user' 또는 'admin'이어야 해요" });

            // 이메일 중복 확인
            if (_users.Any(u => u.Email == user.Email))
                return BadRequest(new { message = "이미 사용 중인 이메일이에요" });

            // ID 자동 부여
            user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);

            _logger.LogInformation("유저 추가: {Name} ({Email})", user.Name, user.Email);

            // 201 Created + Location 헤더 자동 설정
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }


        // ─────────────────────────────────────────
        // PUT /api/users/{id}
        // 유저 정보 수정
        // ─────────────────────────────────────────
        /// <summary>유저 정보를 수정합니다</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Update(int id, [FromBody] User updated)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = $"ID {id}인 유저가 없어요" });

            // 입력값 검증
            if (string.IsNullOrEmpty(updated.Name))
                return BadRequest(new { message = "이름은 필수예요" });

            if (string.IsNullOrEmpty(updated.Email) || !updated.Email.Contains("@"))
                return BadRequest(new { message = "이메일 형식이 올바르지 않아요" });

            if (updated.Age < 1 || updated.Age > 120)
                return BadRequest(new { message = "나이는 1~120 사이여야 해요" });

            // 실제 수정
            user.Name  = updated.Name;
            user.Email = updated.Email;
            user.Age   = updated.Age;
            user.Role  = updated.Role;

            _logger.LogInformation("유저 수정: ID {Id}", id);
            return Ok(user);
        }


        // ─────────────────────────────────────────
        // DELETE /api/users/{id}
        // 유저 삭제
        // ─────────────────────────────────────────
        /// <summary>유저를 삭제합니다</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = $"ID {id}인 유저가 없어요" });

            _users.Remove(user);

            _logger.LogInformation("유저 삭제: {Name} (ID: {Id})", user.Name, id);
            return Ok(new { message = $"{user.Name} 삭제 완료" });
        }


        // ─────────────────────────────────────────
        // GET /api/users/search?name=Sabin
        // 이름으로 검색
        // ─────────────────────────────────────────
        /// <summary>이름으로 유저를 검색합니다</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public ActionResult<List<User>> Search([FromQuery] string? name)
        {
            if (string.IsNullOrEmpty(name))
                return Ok(_users);

            var result = _users
                .Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }
    }
}