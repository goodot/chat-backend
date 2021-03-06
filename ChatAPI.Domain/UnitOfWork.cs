﻿using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Domain.Repository;
using ChatAPI.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _dbContext;
        public UnitOfWork(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(_dbContext);
            MessageRepository = new MessageRepository(_dbContext);
            RoomRepository = new RoomRepository(_dbContext);
            SocketRepository = new SocketRepository(_dbContext);
        }

        public UnitOfWork(IUserRepository userRepository, IRoomRepository roomRepository)
        {
            UserRepository = userRepository;
            RoomRepository = roomRepository;
        }

        public UnitOfWork(IMessageRepository messageRepository)
        {
            MessageRepository = messageRepository;
        }
        public IMessageRepository MessageRepository { get; }
        public IUserRepository UserRepository { get; }
        public IRoomRepository RoomRepository { get; }
        public SocketRepository SocketRepository { get; }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
