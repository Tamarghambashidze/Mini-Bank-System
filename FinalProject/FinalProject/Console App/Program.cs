using FinalProject.Database;
using FinalProject.Extentsions;
using FinalProject.Repsitories;
using FinalProject.Services;

BankSyStemDbContext context = new BankSyStemDbContext();
ClientRepository clientRepository = new ClientRepository(context);
AccountRepository accountRepository = new AccountRepository(context);
TransactionRepository transactionRepository = new TransactionRepository(context);
AnalyticsRepository analyticsRepository = new AnalyticsRepository(context);
FileSystem fileSystem = new FileSystem(context);
do
{
    Console.WriteLine("-------- Bank Sytem -----------");
    Console.Write("1. Client Management\n2. Account Management" +
        "\n3. Transactions\n4. Analytics\n5. File Management\n6. Exit\n Answer: ");
    int answer = Console.ReadLine().IntParse();
    switch (answer)
    {
        case 1:
            new ClientManagement(clientRepository).ClientManagementSyStem();
            break;
        case 2:
            new AccountManagement(accountRepository).AccountManagementSystem(clientRepository);
            break;
        case 3:
            new TransactionManagement(transactionRepository).TransactionManagementSystem(accountRepository);
            break;
        case 4:
            new Analytics(analyticsRepository).AnalyticsSystem();
            break;
        case 5:
            new FileManagement(fileSystem).FileManagementSystem();
            break;
        case 6:
            Console.WriteLine("Exit");
            return;
        default:
            Console.WriteLine("Incorrect input");
            continue;

    }
} while (true);