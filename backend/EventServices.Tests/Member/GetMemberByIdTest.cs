using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Models.Members;
using Event.Application.Queries.EventMember.GetMemberById;
using Event.Domain.Common;
using Event.Domain.Entities;
using Event.Domain.Repositories;
using EventServices.Tests.Utilities;
using FluentAssertions;
using Moq;

namespace EventServices.Tests.Member
{
    public class GetMemberByIdTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IEventMemberRepository> memberRepositoryMock;
        private readonly GetMemberByIdHandler handler;

        public GetMemberByIdTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
            memberRepositoryMock = new Mock<IEventMemberRepository>();

            unitOfWorkMock
                .Setup(x => x.EventMemberRepository)
                .Returns(memberRepositoryMock.Object);

            handler = new GetMemberByIdHandler(
                unitOfWorkMock.Object, mapperMock.Object);
        }


        [Fact]
        public async Task Handle_GetMember_MemberId()
        {
            // Arrange
            var request = new GetMemberByIdQuery() { MemberId = 1};
            var dbMember = TestInitializer.GetTestEventMember(
                1,
                "test",
                "test",
                "test@mail.ru",
                DateTime.UtcNow.AddYears(-20));

            var expectedResult = new MemberResponse
            {
                Id = 1,
                Email = "test@mail.ru",
                FirstName = "test",
                SecondName = "test",
                BirthDate = DateTime.UtcNow.AddYears(20),
                RegistrationDate = DateTime.UtcNow
            };

            memberRepositoryMock.Setup(x => 
                x.GetEventMember(
                    request.MemberId, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(dbMember);

            mapperMock
                .Setup(x => x.Map<MemberResponse>(dbMember))
                .Returns(expectedResult);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            memberRepositoryMock.Verify(x => x.GetEventMember(
                request.MemberId,
                It.IsAny<CancellationToken>()));
            mapperMock.Verify(x => x.Map<MemberResponse>(dbMember));
        }

        [Fact]
        public async Task Handle_NotFound_ThrowNotFoundException()
        {
            // Arrange
            var request = new GetMemberByIdQuery { MemberId = 1 };
            memberRepositoryMock.Setup(x => 
                x.GetEventMember(
                    request.MemberId, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((EventMember)null);

            // Act and Assert
            var ex = async () => await handler.Handle(
                request, 
                CancellationToken.None);

            // Assert
            await ex.Should()
                .ThrowAsync<NotFoundApiException>()
                .WithMessage("Not Found Member");
        }
    }
}
