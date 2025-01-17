﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Portal.Web.Models.TeacherViews;

namespace OtripleS.Portal.Web.Services.TeacherViews
{
    public interface ITeacherViewService
    {
        ValueTask<List<TeacherView>> RetrieveAllTeachersAsync();
    }
}
