using CommBank.Models;
using MongoDB.Driver;

namespace CommBank.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IMongoDatabase database)
    {
        var usersCollection = database.GetCollection<User>("Users");
        var accountsCollection = database.GetCollection<Account>("Accounts");
        var goalsCollection = database.GetCollection<Goal>("Goals");
        var tagsCollection = database.GetCollection<Tag>("Tags");
        var transactionsCollection = database.GetCollection<Transaction>("Transactions");

        var existingUser = await usersCollection.Find(u => u.Id == "62a29c15f4605c4c9fa7f306").FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return;
        }

        await database.DropCollectionAsync("Users");
        await database.DropCollectionAsync("Accounts");
        await database.DropCollectionAsync("Goals");
        await database.DropCollectionAsync("Tags");
        await database.DropCollectionAsync("Transactions");

        var tags = new List<Tag>
        {
            new Tag { Id = "62a39d27025ca1ba8f1f1c1e", Name = "Groceries" },
            new Tag { Id = "62a39d42025ca1ba8f1f1c1f", Name = "Restaurant" },
            new Tag { Id = "62a39d4e025ca1ba8f1f1c20", Name = "Income" },
            new Tag { Id = "62a39d5a025ca1ba8f1f1c21", Name = "Gas" },
            new Tag { Id = "62a39d63025ca1ba8f1f1c22", Name = "Investment" }
        };
        await tagsCollection.InsertManyAsync(tags);

        var accounts = new List<Account>
        {
            new Account
            {
                Id = "62a3e6aad25715026d1a2938",
                Number = 123456789,
                Name = "Tag's Goal Saver",
                Balance = 6483.81,
                AccountType = AccountType.GoalSaver,
                TransactionIds = new List<string>
                {
                    "62a3a284d07648900df72860", "62a3a2ded07648900df72861", "62a3a2ded07648900df72862",
                    "62a3a2ded07648900df72863", "62a3a2ded07648900df72864", "62a3a2ded07648900df72865",
                    "62a3a2ded07648900df72866", "62a3a2ded07648900df72867", "62a3a2ded07648900df72868",
                    "62a3a2ded07648900df72869", "62a3a344d07648900df7286a", "62a3a344d07648900df7286b",
                    "62a3a344d07648900df7286c", "62a3a344d07648900df7286d"
                }
            }
        };
        await accountsCollection.InsertManyAsync(accounts);

        var goals = new List<Goal>
        {
            new Goal
            {
                Id = "62a3f587102e921da1253d32",
                Name = "House Down Payment",
                TargetAmount = 100000,
                TargetDate = DateTimeOffset.FromUnixTimeMilliseconds(1736312400000).DateTime,
                Balance = 73501.82,
                Created = DateTimeOffset.FromUnixTimeMilliseconds(1654912390857).DateTime,
                UserId = "62a29c15f4605c4c9fa7f306"
            },
            new Goal
            {
                Id = "62a3f5e0102e921da1253d33",
                Name = "Tesla Model Y",
                TargetAmount = 60000,
                TargetDate = DateTimeOffset.FromUnixTimeMilliseconds(1662004800000).DateTime,
                Balance = 43840.02,
                Created = DateTimeOffset.FromUnixTimeMilliseconds(1654912480950).DateTime,
                UserId = "62a29c15f4605c4c9fa7f306"
            },
            new Goal
            {
                Id = "62a3f62e102e921da1253d34",
                Name = "Trip to London",
                TargetAmount = 3500,
                TargetDate = DateTimeOffset.FromUnixTimeMilliseconds(1659412800000).DateTime,
                Created = DateTimeOffset.FromUnixTimeMilliseconds(1654912558236).DateTime,
                Balance = 753.89,
                UserId = "62a29c15f4605c4c9fa7f306"
            },
            new Goal
            {
                Id = "62a61945fa15f1cd18516a5f",
                Name = "Trip to NYC",
                TargetAmount = 800,
                TargetDate = DateTimeOffset.FromUnixTimeMilliseconds(1702184400000).DateTime,
                Balance = 0,
                Created = DateTimeOffset.FromUnixTimeMilliseconds(1655053065668).DateTime,
                UserId = "62a29c15f4605c4c9fa7f306"
            }
        };
        await goalsCollection.InsertManyAsync(goals);

        var transactions = new List<Transaction>
        {
            new Transaction { Id = "62a3a284d07648900df72860", TransactionType = TransactionType.Debit, Amount = 135.39, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654891140391).DateTime, TagIds = new[] { "62a39d27025ca1ba8f1f1c1e" }, Description = "Whole Foods", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a2ded07648900df72861", TransactionType = TransactionType.Debit, Amount = 139.26, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654027230566).DateTime, TagIds = new[] { "62a39d27025ca1ba8f1f1c1e" }, Description = "Whole Foods", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a2ebd07648900df72862", TransactionType = TransactionType.Debit, Amount = 26.39, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654891243091).DateTime, TagIds = new[] { "62a39d42025ca1ba8f1f1c1f" }, Description = "Chipotle", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a2ecd07648900df72863", TransactionType = TransactionType.Debit, Amount = 21.9, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654027230566).DateTime, TagIds = new[] { "62a39d42025ca1ba8f1f1c1f" }, Description = "Chipotle", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a316d07648900df72864", TransactionType = TransactionType.Credit, Amount = 5622.81, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654891286080).DateTime, TagIds = new[] { "62a39d4e025ca1ba8f1f1c20" }, Description = "Dropbox", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a318d07648900df72865", TransactionType = TransactionType.Credit, Amount = 5622.92, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654027230566).DateTime, TagIds = new[] { "62a39d4e025ca1ba8f1f1c20" }, Description = "Dropbox", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a323d07648900df72866", TransactionType = TransactionType.Credit, Amount = 1439.18, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654891299481).DateTime, TagIds = new[] { "62a39d4e025ca1ba8f1f1c20" }, Description = "Fencer", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a324d07648900df72867", TransactionType = TransactionType.Credit, Amount = 1439.89, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654027230566).DateTime, TagIds = new[] { "62a39d4e025ca1ba8f1f1c20" }, Description = "Fencer", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a337d07648900df72868", TransactionType = TransactionType.Debit, Amount = 44.52, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654891319411).DateTime, TagIds = new[] { "62a39d5a025ca1ba8f1f1c21" }, Description = "Gas", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a338d07648900df72869", TransactionType = TransactionType.Debit, Amount = 44.13, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654027230566).DateTime, TagIds = new[] { "62a39d5a025ca1ba8f1f1c21" }, Description = "Gas", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a344d07648900df7286a", TransactionType = TransactionType.Debit, Amount = 1500, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654891332111).DateTime, TagIds = new[] { "62a39d63025ca1ba8f1f1c22" }, Description = "Coinbase", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a344d07648900df7286b", TransactionType = TransactionType.Debit, Amount = 1500, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654027230566).DateTime, TagIds = new[] { "62a39d63025ca1ba8f1f1c22" }, Description = "Coinbase", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a348d07648900df7286c", TransactionType = TransactionType.Debit, Amount = 1500, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654891336929).DateTime, TagIds = new[] { "62a39d63025ca1ba8f1f1c22" }, Description = "Titan", UserId = "62a29c15f4605c4c9fa7f306" },
            new Transaction { Id = "62a3a349d07648900df7286d", TransactionType = TransactionType.Debit, Amount = 1500, DateTime = DateTimeOffset.FromUnixTimeMilliseconds(1654027230566).DateTime, TagIds = new[] { "62a39d63025ca1ba8f1f1c22" }, Description = "Titan", UserId = "62a29c15f4605c4c9fa7f306" }
        };
        await transactionsCollection.InsertManyAsync(transactions);

        var user = new User
        {
            Id = "62a29c15f4605c4c9fa7f306",
            Name = "Tag Ramotar",
            Email = "tag@dropbox.com",
            Password = "$2a$11$10VhY5XIwBeWA4uLIE.sr.c34UvwLRQPD8yy7z/4iiN6ez5z2Pg1S",
            AccountIds = new List<string> { "62a3e6aad25715026d1a2938" },
            GoalIds = new List<string> { "62a3f587102e921da1253d32", "62a3f5e0102e921da1253d33", "62a3f62e102e921da1253d34", "62a61945fa15f1cd18516a5f" },
            TransactionIds = new List<string>
            {
                "62a3a284d07648900df72860", "62a3a2ded07648900df72861", "62a3a2ebd07648900df72862", "62a3a2ebd07648900df72863", "62a3a2ebd07648900df72864",
                "62a3a2ebd07648900df72865", "62a3a2ebd07648900df72866", "62a3a2ebd07648900df72867", "62a3a2ebd07648900df72868", "62a3a2ebd07648900df72869",
                "62a3a344d07648900df7286a", "62a3a344d07648900df7286b", "62a3a348d07648900df7286c", "62a3a349d07648900df7286d"
            }
        };
        await usersCollection.InsertOneAsync(user);
    }
}
