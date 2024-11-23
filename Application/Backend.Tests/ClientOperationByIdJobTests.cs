using AutoMapper;
using Moq;

using Backend.Configs;
using Backend.Jobs;

using Common.Queue.Message;
using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message.ClientOperation.Result;

using DAL;
using DAL.Enteties.ClientOperations.Result;
using DAL.Respositories;

namespace Backend.Tests
{
    public class ClientOperationByIdJobTests
    {
        private readonly IMapper _mapper = AutoMapperConfig.GetMapper();

        [Fact]
        public void Thtow_Exceptions_When_Constructor_Arguments_Is_Null()
        {
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();

            Assert.Throws<ArgumentNullException>(() => new GetClientOperationJob((null)!, mockMapper.Object));
            Assert.Throws<ArgumentNullException>(() => new GetClientOperationJob(mockUnitOfWork.Object, (null)!));
        }

        [Fact]
        public async Task Get_CientOperaion_By_Id_AND_Return_Null_When_No_Record()
        {
            Guid clientOperationIdInProc = Guid.NewGuid();
            Guid clientOperationIdExpected = Guid.NewGuid();

            IUnitOfWork unitOfWork = GetUnitOfWork(clientOperationIdInProc, clientOperationIdExpected);

            IJob job = new GetClientOperationJob(unitOfWork, _mapper);

            BaseMessage? baseMessage = await job.ExecuteAsync(new ClientOperationRequestMessage()
            {
                ClientOperationId = clientOperationIdExpected,
            });

            Assert.Null(baseMessage);
        }

        [Fact]
        public async Task Get_CientOperaion_By_Id_AND_Return_Result_When_Record_Exist()
        {
            Guid clientOperationIdInProc = Guid.NewGuid();
            Guid clientOperationIdWithNull = Guid.NewGuid();
            Guid clientOperationIdExpected = clientOperationIdInProc;

            IUnitOfWork unitOfWork = GetUnitOfWork(clientOperationIdInProc, clientOperationIdWithNull);

            IJob job = new GetClientOperationJob(unitOfWork, _mapper);

            BaseMessage? baseMessage = await job.ExecuteAsync(new ClientOperationRequestMessage()
            {
                ClientOperationId = clientOperationIdExpected,
            });

            Assert.NotNull(baseMessage);
            Assert.IsType<ClientOperationResultMessage>(baseMessage);

            ClientOperationResultMessage? resultMessage = baseMessage as ClientOperationResultMessage;

            Assert.NotNull(resultMessage);
            Assert.Equal(clientOperationIdExpected, clientOperationIdInProc);
            Assert.NotEqual(clientOperationIdExpected, clientOperationIdWithNull);
            Assert.Equal(resultMessage.ClientOperationId, clientOperationIdExpected);
        }

        private IUnitOfWork GetUnitOfWork(Guid clientOperationIdInProc, Guid clientOperationIdWithNull)
        {
            ClientOperationResult? clietnOperationResult = new ClientOperationResult()
            {
                ClientOperationId = clientOperationIdInProc,
            };

            Mock<IClientOperationRepository> mockClientOperationRepository = new Mock<IClientOperationRepository>();
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

            mockClientOperationRepository
                .Setup(x => x.GetClientOperationById(clientOperationIdInProc))
                .Returns(Task.FromResult<ClientOperationResult?>(clietnOperationResult));
            mockClientOperationRepository
                .Setup(x => x.GetClientOperationById(clientOperationIdWithNull))
                .Returns(Task.FromResult<ClientOperationResult?>(null));

            mockUnitOfWork
                .Setup(x => x.ClientOperationRepository)
                .Returns(mockClientOperationRepository.Object);

            return mockUnitOfWork.Object;
        }
    }
}