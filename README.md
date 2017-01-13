판매자와 구매자만이 아이템 거래시 인벤토리의 참조할 수 있도록 하는것이 목표이다.
`ISeller`, `IBuyer`, `ITransaction` 인터페이스는 거래자들의 소지아이템을 수정 할 수 있는 `public setter`(예] `int Gold { get; set; }`)가 존재 하지 않는다.
`ISeller.BeginTransaction` 및 `IBuyer.Approve`함수를 통해 ITransaction을 상속 받는 객체에게 참조를 넘겨준다.
`ITransaction`을 상속 받는 객체는 `Commit` 함수를 통해 넘겨받은 참조를 이용하여 거래를 진행한다.