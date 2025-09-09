using CarManagementApp.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace CarManagementApp.Services.JWT
{
    public interface IJwtGenerator
    {
        (string token, DateTimeOffset expiresAtUtc) CreateToken(Kierowca user, IEnumerable<Claim> claims);
    }
}