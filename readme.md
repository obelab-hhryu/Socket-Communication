- - ## Socket Protocol
  
    #### 1. Specification
  
    - IP: localhost
    - Port: 5555
  
    #### 2. Data format
  
    ##### 1) Send/Receive Socket data format
  
    - **SocketData**
      - Type: Enum (DataType)
        - 0 ~ 7 (아래 참조)
      - CommonConfig (Type: 0)
      - RacingConfig (Type: 1)
      - BasketballConfig (Type: 2)
      - EggConfig (Type: 3)
      - MeasurementStage (Type: 4)
      - Measurement (Type: 5)
      - Result (Type: 6)
      - Result (Type: 7)
  
    
  
    ##### 2) Object
  
    - **CommonConfig (공통 게임 설정)**
      - Users: UserInfo[] (사용자 정보)
        - UserInfo (사용자 정보)
          - Name : String (이름 or ID)
          - School : String (학교명)
      - PlayerCount : Int (피험자 수)
      - Sensitivity : Int (민감도: 1 ~ 10)
      - WatingTime: Int (대기시간: sec)
      - PlayTime: int (측정시간: sec)
      - ResultTime: Int (결과 로드 시간: sec)
    - **RacingConfig (레이싱 설정)**
      - MinSpeed: Int
      - MaxSpeed: Int
    - **BasketballConfig (농구 설정)**
      - MaxShootingCount: int (최대 슈팅 횟수)
      - ShootingPositions: ShootingPosition[]
        - ShootingPosition (슈팅 위치)
          - X: double
          - Y: double
          - Z: double
          - StandardValue: Int (골인 뇌파 기준값)
    - **EggConfig (알 키우기 설정)**
      - 미정
    - **Measurement (측정 전달 값)**
      - UserDataArray: UserData[]
        - Name: String
        - School: String
        - Value: int
    - **MeasurementStage (측정 진행 상태)**
      - Stage: Enum
        - Started (0): 측정 시작 (측정 시작 전 전송)
        - Finished (1): 측정 완료  (측정 완료  후 전송)
    - **UserScore (사용자 별 게임 결과)**
      - User: UserInfo (사용자 정보)
      - Score:  Double (게임에 따른 점수)
    - **Result (모든 사용자 결과 정보)**
      - UserScores: UserScore[]
        - User: UserInfo
          - Name: String 
          - School: String
        - Score: Double (점수)
    - **Rank (순위 정보)**
      - BestArray: UserRank[]
        - User: UserInfo
          - Name: String 
          - School: String
        - Ranking: int
      - WorstArray: UserRank[]
      - PlayerArray: UserRank[]
  
    #### 3. Sample source
  
    - SampleSource/SocketServer
  
    #### 4. Reference (SocketServer/Document)
  
    - Flowchart
    - JSON 파일 샘플
