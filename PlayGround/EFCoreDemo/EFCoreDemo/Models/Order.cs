namespace EFCoreDemo.Models;

    public class Order
    {
        // ═══════════════════════════════════════════════════════════════
        // 🔥 핵심 6: Foreign Key (FK) 자동 인식
        // ═══════════════════════════════════════════════════════════════
        // ❌ 기존: FOREIGN KEY 제약조건을 SQL로 정의
        //   FOREIGN KEY (UserId) REFERENCES Users(Id)
        // Foreign key의 의미는 "이 주문은 어떤 유저의 것인가?"를 나타내는 참조입니다. 즉, Orders 테이블의 UserId 칼럼은 Users 테이블의 Id 칼럼을 참조하는 외래 키입니다.
        // 쉽게 말해, Orders 테이블의 UserId는 Users 테이블의 Id와 연결되어 있어서, 어떤 주문이 어떤 유저의 것인지 알 수 있게 해줍니다.
        
        // ✅ EF Core: 명명 규칙으로 자동 인식!
        // - UserId라는 프로퍼티 + User 네비게이션 프로퍼티 → 자동으로 FK 설정
        
        public int Id { get; set; }

        public string Product { get; set; } = "";

        public int Price { get; set; }

        // 어떤 유저의 주문인지 (Users 테이블의 Id 참조)
        // - 명명 규칙: User 엔티티이름 + Id → EF Core가 FK로 인식
        public int UserId { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 🔥 핵심 7: Navigation Property = 객체를 통한 관계 접근
        // ═══════════════════════════════════════════════════════════════
        // ❌ 기존: UserId 숫자로만 접근 → 유저 정보 필요시 별도 쿼리 필요
        //   int userId = order.UserId;
        //   User user = GetUserById(userId); // 추가 쿼리
        
        // ✅ EF Core: order.User로 바로 접근!
        // - 내부적으로 JOIN 자동 처리
        // - Lazy Loading (지연 로딩) 지원 → 필요할 때만 로드
        public User User { get; set; } = null!;
    }