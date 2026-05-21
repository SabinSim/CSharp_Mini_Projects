using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Models;

namespace EFCoreDemo.Data
{
    // ═══════════════════════════════════════════════════════════════
    // 🔥 핵심 1: DbContext = DB 연결의 중심
    // ═══════════════════════════════════════════════════════════════
    // ❌ 기존: SqlConnection, SqlCommand, SqlDataReader 직접 생성/관리 → 코드 복잡
    // ✅ EF Core: DbContext 상속 → 모든 DB 작업이 통일된 인터페이스로 관리됨
    // DbContext는 "ORM(Object-Relational Mapping)"의 핵심 = 객체 ↔ 테이블 자동 변환
    // DbContext에 상속시킨다는 의미는 EF Core로 DB 작업을 하겠다는 선언
    // → EF Core가 내부적으로 DB 연결, 쿼리 실행, 결과 매핑 등을 자동으로 처리
    public class AppDbContext : DbContext 
    {
        // ═══════════════════════════════════════════════════════════════
        // 🔥 핵심 2: DbSet = DB 테이블을 C# 객체로 매핑
        // ═══════════════════════════════════════════════════════════════
        // ❌ 기존: SELECT * FROM Users → SqlDataReader로 수동 매핑 필요
        //   var users = new List<User>();
        //   while (reader.Read()) {
        //       users.Add(new User { Id = reader.GetInt32(0), Name = reader.GetString(1) ... });
        //   }
        
        // ✅ EF Core: _db.Users로 접근 → 자동으로 User 객체로 변환!
        
        // DbSet은 "Entity Set"의 약자 = 특정 엔티티(User, Order 등)에 대한 DB 테이블을 나타냄
        // 엔티티란 DB 테이블과 매핑되는 C# 클래스 (User, Order 등) 왜 엔티티라고 부르냐면, DB 테이블의 행(row)이 C# 객체(instance)로 표현되기 때문
        public DbSet<User> Users { get; set; }  // Users 테이블 = User 객체 컬렉션
        public DbSet<Order> Orders { get; set; } // Orders 테이블 = Order 객체 컬렉션

        // ═══════════════════════════════════════════════════════════════
        // 🔥 핵심 3: 의존성 주입(DI) = 설정 외부화
        // ═══════════════════════════════════════════════════════════════
        // ❌ 기존: 연결 문자열을 코드에 하드코딩 → 보안, 유지보수 문제
        // ✅ EF Core: Program.cs에서 설정 주입받음 → 환경별로 다른 DB 사용 가능
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}