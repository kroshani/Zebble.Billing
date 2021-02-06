﻿namespace Zebble.Billing
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Olive;

    class SubscriptionRepository : ISubscriptionRepository
    {
        readonly BillingDbContext Context;

        public SubscriptionRepository(BillingDbContext context) => Context = context;

        public Task<Subscription> GetByPurchaseToken(string purchaseToken)
        {
            return Context.Subscriptions.SingleOrDefaultAsync(x => x.PurchaseToken == purchaseToken);
        }

        public Task<Subscription> GetMostUpdatedByUserId(string userId)
        {
            return Context.Subscriptions.Where(x => x.UserId == userId)
                                         .Where(x => x.DateSubscribed <= LocalTime.Now)
                                         .Where(x => x.ExpiryDate >= LocalTime.Now)
                                         .Where(x => x.CancellationDate == null || x.CancellationDate >= LocalTime.Now)
                                         .OrderBy(x => x.ExpiryDate)
                                         .LastOrDefaultAsync();
        }

        public async Task<Subscription> AddSubscription(Subscription subscription)
        {
            await Context.Subscriptions.AddAsync(subscription);
            await Context.SaveChangesAsync();

            return subscription;
        }

        public async Task UpdateSubscription(Subscription subscription)
        {
            Context.Subscriptions.Update(subscription);
            await Context.SaveChangesAsync();
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            await Context.Transactions.AddAsync(transaction);
            await Context.SaveChangesAsync();

            return transaction;
        }
    }
}
