﻿using Models;

namespace Services
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<int> CreateAsync(User user);
    }
}
