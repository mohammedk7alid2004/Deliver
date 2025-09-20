﻿using Deliver.Entities.Entities;
namespace Deliver.BLL.Authentication;
public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
    string? ValidateToken(string token);
}
