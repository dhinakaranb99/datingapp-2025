using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Extenstions;

public static class AppUserExtenstions
{
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        };
    }
}
