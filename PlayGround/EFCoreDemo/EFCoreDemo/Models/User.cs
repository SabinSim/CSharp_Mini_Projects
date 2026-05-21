namespace EFCoreDemo.Models
{
    public class User
    {
        // ═══════════════════════════════════════════════════════════════
        // 🔥 핵심 4: 프로퍼티 = DB 테이블 칼럼 (자동 변환!)
        // ═══════════════════════════════════════════════════════════════
        // ❌ 기존: CREATE TABLE Users (Id INT PRIMARY KEY, Name TEXT, ...) SQL 직접 작성
        // ✅ EF Core: C# 클래스만 정의 → 자동으로 테이블 생성!
        
        // PK (Primary Key: 기본 키) 자동 인식
        // - 프로퍼티 이름이 "Id" → EF Core가 자동으로 Primary Key로 인식
        // - Auto Increment (자동 증가) 자동 설정
        public int Id { get; set; }
        
        public string Name { get; set; } = "";      // NOT NULL
        public string Email { get; set; } = "";     // NOT NULL
        public int Age { get; set; }                // NOT NULL

        // ═══════════════════════════════════════════════════════════════
        // 🔥 핵심 5: Navigation Property = 관계 매핑 (JOIN 자동화!)
        // ═══════════════════════════════════════════════════════════════
        // ❌ 기존: 복잡한 JOIN 쿼리 직접 작성
        //   SELECT u.*, o.* FROM Users u LEFT JOIN Orders o ON u.Id = o.UserId WHERE u.Id = 1
        
        // ✅ EF Core: 객체 참조로 표현 → 관계 자동 관리
        // - user.Orders로 그 유저의 모든 주문에 접근 가능
        // - 1:N 관계: 한 유저가 여러 주문을 가짐
        public List<Order> Orders { get; set; } = new(); // 주문 목록
    }
}