﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using OtripleS.Portal.Web.Models.Students;
using OtripleS.Portal.Web.Models.Students.Exceptions;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Portal.Web.Tests.Unit.Services.Students
{
    public partial class StudentServiceTests
    {
        public static TheoryData ValidationApiExceptions()
        {
            string exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseBadRequestException =
                new HttpResponseBadRequestException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var httpResponseConflictException =
                new HttpResponseConflictException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            return new TheoryData<Exception>
            {
                httpResponseBadRequestException,
                httpResponseConflictException
            };
        }

        [Theory]
        [MemberData(nameof(ValidationApiExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRegisterIfBadRequestErrorOccursAndLogItAsync(
            Exception validationApiException)
        {
            // given
            Student someStudent = CreateRandomStudent();

            var expectedDepndencyValidationException =
                new StudentDependencyValidationException(
                    validationApiException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(validationApiException);

            // when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(someStudent);

            // then
            await Assert.ThrowsAsync<StudentDependencyValidationException>(() =>
               registerStudentTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedDepndencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        public static TheoryData CriticalApiExceptions()
        {
            string exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpRequestException =
                new HttpRequestException();

            var httpResponseUrlNotFoundException =
                new HttpResponseUrlNotFoundException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var httpResponseUnAuthorizedException =
                new HttpResponseUnauthorizedException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            return new TheoryData<Exception>
            {
                httpRequestException,
                httpResponseUrlNotFoundException,
                httpResponseUnAuthorizedException
            };
        }

        [Theory]
        [MemberData(nameof(CriticalApiExceptions))]
        public async Task ShouldThrowCriticalDependencyExceptionOnRegisterIfUrlNotFoundErrorOccursAndLogItAsync(
            Exception httpResponseCriticalException)
        {
            // given
            Student someStudent = CreateRandomStudent();

            var expectedStudentDepndencyException =
                new StudentDependencyException(
                    httpResponseCriticalException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(httpResponseCriticalException);

            // when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(someStudent);

            // then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
               registerStudentTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(
                    SameExceptionAs(expectedStudentDepndencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        public static TheoryData DependencyApiExceptions()
        {
            string exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseException =
                new HttpResponseException(
                    httpResponseMessage: responseMessage,
                    message: exceptionMessage);

            var httpResponseInternalServerErrorException =
                new HttpResponseInternalServerErrorException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            return new TheoryData<Exception>
            {
                httpResponseException,
                httpResponseInternalServerErrorException
            };
        }

        [Theory]
        [MemberData(nameof(DependencyApiExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRegisterIfDependencyApiErrorOccursAndLogItAsync(
            Exception dependencyApiException)
        {
            // given
            Student someStudent = CreateRandomStudent();
            string exceptionMessage = GetRandomString();

            var expectedStudentDepndencyException =
                new StudentDependencyException(
                    dependencyApiException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(dependencyApiException);

            // when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(someStudent);

            // then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
               registerStudentTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedStudentDepndencyException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRegisterIfErrorOccursAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();
            var serviceException = new Exception();

            var expectedStudentServiceException =
                new StudentServiceException(serviceException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(someStudent);

            // then
            await Assert.ThrowsAsync<StudentServiceException>(() =>
               registerStudentTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostStudentAsync(It.IsAny<Student>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedStudentServiceException))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
