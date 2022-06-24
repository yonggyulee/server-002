# 통합 테스트 프로젝트 

DB, MessageBroker 등 추가적인 서비스들은 Docker Container로 띄우고, Grpc 서비스는 Test Client를 사용하고 있음
(TODO 향후, `Mirero.DAQ.Service.Api` 를 Docker Container로 띄우고 Production Grpc Client를 사용할 수도 있음)

## DB DDL 스크립트 생성 

`generate-ddl-script.cmd` 를 실행하면 "./sql" 디렉토리에 다음과 같은 파일이 생성된다. 

- `./sql/account-db.sql` 
- `./sql/dataset-db.sql` 

이 작업은 DB Schema 가 변경된 경우 통합테스트를 실행하기 전에 실행하며, 
생성된 파일은 커밋을 해 둔다(통합 테스트를 하기 전 매번 DDL을 생성할 필요는 없으므로)


## 통합 테스트의 재료

- TestContainers : https://github.com/HofmeisterAn/dotnet-testcontainers
   - RabbitMqTestContainer : [코드](https://github.com/HofmeisterAn/dotnet-testcontainers/blob/develop/src/DotNet.Testcontainers/Containers/Modules/MessageBrokers/RabbitMqTestcontainer.cs) 참고
   - PostgreSqlTestcontainer : [코드](https://github.com/HofmeisterAn/dotnet-testcontainers/blob/develop/src/DotNet.Testcontainers/Containers/Modules/Databases/PostgreSqlTestcontainer.cs)
- C# Grpc Integration Test : [MS 공식문서](https://docs.microsoft.com/en-us/aspnet/core/grpc/test-services?view=aspnetcore-6.0#integration-test-grpc-services)
- xUnit
 - [테스트들 간 공유 컨텍스트 개념](https://xunit.net/docs/shared-context) : 컨테이너는 전체 테스트 시나리오에서 한번 생성되고, 일련의 테스트들에 공유되어야 한다(시나리오 테스트를 위함)
 - [테스트 순서](https://stackoverflow.com/questions/9210281/how-to-set-the-test-case-sequence-in-xunit) : 통합 테스트의 경우, 시나리오 테스트인경우가 많으므로, 테스트의 순서가 중요한 경우가 많다
