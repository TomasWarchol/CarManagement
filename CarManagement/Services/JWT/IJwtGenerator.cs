using CarManagement.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace CarManagement.Services.JWT
{
    public interface IJwtGenerator
    {
        (string token, DateTime expiresAtUtc) CreateToken(Kierowca user, IEnumerable<Claim> claims);
    }
}