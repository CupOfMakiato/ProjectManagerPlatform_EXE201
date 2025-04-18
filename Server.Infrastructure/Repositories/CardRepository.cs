﻿using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Repositories
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        private readonly AppDbContext _dbContext;

        public CardRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetTotalCardCount(CardStatus? status = null)
        {
            var query = _dbContext.Cards.Where(c => !c.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> CountAttachmentsInACard(Guid cardId)
        {
            return await _dbContext.Attachments
                .Where(a => a.CardId == cardId)
                .CountAsync();
        }


        public async Task<List<Card>> GetAllCards()
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Include(s => s.CardCreatedByUser).ToListAsync();
        }

        public async Task<List<Card>> GetAllOpenCards()
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .Where(c => c.Status == CardStatus.Open)
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Include(s => s.CardCreatedByUser).ToListAsync();
        }

        public async Task<List<Card>> GetAllArchivedCards()
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .Where(c => c.Status == CardStatus.Closed)
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Include(s => s.CardCreatedByUser).ToListAsync();
        }

        public async Task<List<Card>> GetCardsByBoardId(Guid boardId)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .Where(c => !c.IsDeleted && c.Column.BoardId == boardId) // Filter by boardId
                .OrderByDescending(c => c.ModificationDate) // Sort by last update
                .Include(s => s.CardCreatedByUser)
                .ToListAsync();
        }

        public async Task<List<Card>> GetArchivedCardsByBoardId(Guid boardId)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include Column to access BoardId
                .ThenInclude(col => col.Board) // Include Board for verification
                .Include(c => c.Attachments)
                .Where(c => c.Status == CardStatus.Closed)
                .Where(c => !c.IsDeleted && c.Column.BoardId == boardId) // Filter by boardId
                .OrderByDescending(c => c.ModificationDate) // Sort by last update
                .Include(s => s.CardCreatedByUser)
                .ToListAsync();
        }


        public async Task<Card?> GetCardById(Guid id)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) 
                .ThenInclude(col => col.Board) 
                .Include(c => c.Attachments) 
                .Include(c => c.CardCreatedByUser) 
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted); 
        }

        public async Task<List<Card>> GetCardsByColumnId(Guid columnId)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include the column to ensure relational integrity
                .Include(c => c.Attachments) // Include attachments if needed
                .Where(c => !c.IsDeleted && c.ColumnId == columnId) // Filter by columnId and exclude deleted cards
                .OrderBy(c => c.CardPosition) // Order by position within the column
                .ToListAsync();
        }

        public async Task<List<Card>> GetOpenCardsByColumnId(Guid columnId)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include the column to ensure relational integrity
                .Include(c => c.Attachments) // Include attachments if needed
                .Where(c => !c.IsDeleted && c.ColumnId == columnId) // Filter by columnId and exclude deleted cards
                .Where(c => c.Status == CardStatus.Open)
                .OrderBy(c => c.CardPosition) // Order by position within the column
                .ToListAsync();
        }
        public async Task<List<Card>> GetArchivedCardsByColumnId(Guid columnId)
        {
            return await _dbContext.Cards
                .Include(c => c.Column) // Include the column to ensure relational integrity
                .Include(c => c.Attachments) // Include attachments if needed
                .Where(c => !c.IsDeleted && c.ColumnId == columnId) // Filter by columnId and exclude deleted cards
                .Where(c => c.Status == CardStatus.Closed)
                .OrderBy(c => c.CardPosition) // Order by position within the column
                .ToListAsync();
        }


        public async Task<List<Card>> GetPagedCards(int pageIndex, int pageSize, CardStatus? status = null)
        {
            var query = _dbContext.Cards
                .Where(c => !c.IsDeleted);

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query
                .OrderByDescending(c => c.ModificationDate)  // Sorting by last updated
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(s => s.CardCreatedByUser)
                .ToListAsync();
        }

        public async Task<List<Card>> GetCardsDueBeforeAsync(DateTime dueDate)
        {
            return await _dbContext.Set<Card>()
                .Where(c => c.DueDate != null && c.DueDate <= dueDate)
                .ToListAsync();
        }

        public async Task<List<Card>> GetCardsWithUpcomingReminders(DateTime now)
        {
            return await _dbContext.Cards
                .Where(card => card.DueDate.HasValue
                    && card.Reminder != ReminderType.None
                    && GetReminderTime(card.DueDate.Value, card.Reminder) <= now
                    && card.DueDate.Value > now) // Ensure it's not overdue
                .ToListAsync();
        }


        private static DateTime GetReminderTime(DateTime dueDate, ReminderType reminder)
        {
            return reminder switch
            {
                ReminderType.FiveMinutes => dueDate.AddMinutes(-5),
                ReminderType.TenMinutes => dueDate.AddMinutes(-10),
                ReminderType.OneHour => dueDate.AddHours(-1),
                ReminderType.OneDay => dueDate.AddDays(-1),
                ReminderType.TwoDays => dueDate.AddDays(-2),
                _ => dueDate // Default (should not happen)
            };
        }

        public async Task<List<Card>> SearchCardsAsync(string textSearch)
        {
            return await _dbContext.Cards
                .Where(s => s.Title.Contains(textSearch)
                //|| s.Description.Contains(textSearch)
                )
                .AsNoTracking()
                .ToListAsync();
        }

        

        // Filter 
        // SOS

    }
}
