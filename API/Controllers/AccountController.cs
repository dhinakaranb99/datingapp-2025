using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{

    [HttpPost("register")]
    public async Task<ActionResult<UserDto  >> Register(RegisterDto registerDto)
    {
        if (await EmailExists(registerDto.Email)) return BadRequest("Email Taken");
        var hmac = new HMACSHA512();
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user.ToDto(tokenService);
    }   

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == logindto.Email);
        if (user == null) return Unauthorized("Invalid email address");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

        for (var i = 0; i < ComputedHash.Length; i++)
        {
            if (ComputedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        return user.ToDto(tokenService);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

}

