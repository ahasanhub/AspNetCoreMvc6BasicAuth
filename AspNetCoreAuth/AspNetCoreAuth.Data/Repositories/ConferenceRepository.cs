﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAuth.Data.Entities;
using AspNetCoreAuth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreAuth.Data.Repositories
{
    public class ConferenceRepository : IConferenceRepository
    {
        private readonly ConfDbContext dbContext;

        public ConferenceRepository(ConfDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<List<ConferenceModel>> GetAll()
        {
            return dbContext.Conferences.Include(c => c.Attendees).Select(c => c.ToModel()).ToListAsync();
        }

        public async Task<ConferenceModel> GetById(int id)
        {
            var entity = await dbContext.Conferences.FirstAsync(c => c.Id == id);
            return entity.ToModel();
        }

        public Task<int> Add(ConferenceModel model)
        {
            var entity = Conference.FromModel(model);
            dbContext.Conferences.Add(entity);
            return dbContext.SaveChangesAsync();
        }
    }
}
