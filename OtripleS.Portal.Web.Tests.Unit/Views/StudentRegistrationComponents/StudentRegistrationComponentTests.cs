﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OtripleS.Portal.Web.Models.StudentViews;
using OtripleS.Portal.Web.Services.StudentViews;
using OtripleS.Portal.Web.Views.Components;
using Tynamix.ObjectFiller;

namespace OtripleS.Portal.Web.Tests.Unit.Views.StudentRegistrationComponents
{
    public partial class StudentRegistrationComponentTests : TestContext
    {
        private readonly Mock<IStudentViewService> studentViewServiceMock;
        private IRenderedComponent<StudentRegistrationComponent> renderedStudentRegistrationComponent;

        public StudentRegistrationComponentTests()
        {
            this.studentViewServiceMock = new Mock<IStudentViewService>();
            this.Services.AddScoped(services => this.studentViewServiceMock.Object);
            this.Services.AddServerSideBlazor();
        }

        private static StudentView CreateRandomStudentView() =>
            CreateStudentFiller().Create();

        private static Filler<StudentView> CreateStudentFiller()
        {
            var filler = new Filler<StudentView>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }
    }
}