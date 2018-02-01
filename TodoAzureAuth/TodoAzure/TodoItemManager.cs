// To add offline sync support: add the NuGet package WindowsAzure.MobileServices.SQLiteStore
// to all projects in the solution and uncomment the symbol definition OFFLINE_SYNC_ENABLED
// For Xamarin.iOS, also edit AppDelegate.cs and uncomment the call to SQLitePCL.CurrentPlatform.Init()
// For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342 

// Enable offline sync for your Xamarin.Forms mobile app
//https://docs.microsoft.com/en-gb/azure/app-service-mobile/app-service-mobile-xamarin-forms-get-started-offline-data
//https://docs.microsoft.com/en-gb/azure/app-service-mobile/app-service-mobile-offline-data-sync


#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace TodoAzure
{
    public class TodoItemManager
    {
#if OFFLINE_SYNC_ENABLED

        // The IMobileServiceSyncTable uses the local database for all create, read, update, and delete (CRUD) table operations. 
        IMobileServiceSyncTable<TodoItem> todoTable;

#else
        private readonly IMobileServiceTable<TodoItem> todoTable;
#endif

        private TodoItemManager()
        {
            CurrentClient = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            // This code creates a new local SQLite database using the MobileServiceSQLiteStore class.
            var store = new MobileServiceSQLiteStore("localstore.db");

            // The DefineTable method creates a table in the local store that matches the fields in the provided type. 
            // The type doesn't have to include all the columns that are in the remote database. It is possible to store a subset of columns.
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.CurrentClient.SyncContext.InitializeAsync(store);

            this.todoTable = CurrentClient.GetSyncTable<TodoItem>();
#else
            todoTable = CurrentClient.GetTable<TodoItem>();
#endif
        }

        public static TodoItemManager DefaultManager => new TodoItemManager();

        public MobileServiceClient CurrentClient { get; }

        public bool IsOfflineEnabled => todoTable is IMobileServiceSyncTable<TodoItem>;

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                var items = await todoTable
                    .Where(todoItem => !todoItem.Done)
                    .ToEnumerableAsync();

                return new ObservableCollection<TodoItem>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return null;
        }

        public async Task SaveTaskAsync(TodoItem item)
        {
            if (item.Id == null)
                await todoTable.InsertAsync(item);
            else
                await todoTable.UpdateAsync(item);
        }

#if OFFLINE_SYNC_ENABLED
        // Sync with the Mobile App backend
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                // Push changes from local todoTable to the Mobile App backend by calling PushAsync on the IMobileServiceSyncContext
                // The sync context helps preserve table relationships by tracking and pushing changes in all tables a client app has modified when PushAsync is called.
                await this.CurrentClient.SyncContext.PushAsync(); // from local SQLite to remote backend easy table with Azure Mobile App.

                // Pull change from backend to local SQLite. 
                await this.todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this.todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}