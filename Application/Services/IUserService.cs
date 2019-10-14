using QAP4.Domain.AggreatesModels.Users.Models;
using System.Collections.Generic;

namespace QAP4.Application.Services
{
    public interface IUserService
    {
        User FindUserById(int userId);
    }
}