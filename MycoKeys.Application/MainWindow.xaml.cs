using MycoKeys.Library.Database;
using PetaPoco.NetCore;
using System;
using System.Windows;

namespace MycoKeys.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewModel.IImporter
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _keyPath = System.Environment.Is64BitOperatingSystem ? @"SOFTWARE\Wow6432Node\MycoKeys.Application\DatabaseSettings" : @"SOFTWARE\MycoKeys.Application\DatabaseSettings";

        private void ShowKeysView(MycoKeys.Library.Database.IDatabase iDatabase, MycoKeys.Library.Database.KeyManager keyManager)
        {
            MycoKeys.Application.View.KeysListView keysListView = new View.KeysListView(this);
            ViewModel.KeysListViewModel keysListViewModel = new ViewModel.KeysListViewModel(iDatabase.Name, keyManager);
            keysListView.DataContext = keysListViewModel;
            keysListViewModel.Load();
            keysListView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            keysListView.Owner = this;
            keysListView.ShowDialog();
            if (keysListView._datagridKeys.Columns.Count > 0)
            {
                keysListView._datagridKeys.Columns[0].Visibility = Visibility.Collapsed;
            }
            iDatabase.CloseConnection();
        }

        private bool OpenDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, out MycoKeys.Library.Database.KeyManager keyManager)
        {
            iDatabase = null;
            keyManager = null;

            OpenControls.Wpf.Serialisation.RegistryItemSerialiser registryItemSerialiser = new OpenControls.Wpf.Serialisation.RegistryItemSerialiser(_keyPath);
            OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration databaseConfiguration = new OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration(registryItemSerialiser);
            if (registryItemSerialiser.OpenKey())
            {
                databaseConfiguration.Load();
            }

            OpenControls.Wpf.DatabaseDialogs.ViewModel.OpenDatabaseViewModel openDatabaseViewModel = new OpenControls.Wpf.DatabaseDialogs.ViewModel.OpenDatabaseViewModel(databaseConfiguration);
            OpenControls.Wpf.DatabaseDialogs.View.OpenDatabaseView openDatabaseView =
                new OpenControls.Wpf.DatabaseDialogs.View.OpenDatabaseView(new OpenControls.Wpf.DatabaseDialogs.Model.Encryption());
            openDatabaseView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            openDatabaseView.Owner = this;
            openDatabaseView.DataContext = openDatabaseViewModel;
            if (openDatabaseView.ShowDialog() != true)
            {
                return false;
            }
            if (!registryItemSerialiser.IsOpen)
            {
                registryItemSerialiser.CreateKey();
            }
            databaseConfiguration.Save();
            registryItemSerialiser.Close();

            try
            {
                OpenControls.Wpf.DatabaseDialogs.Model.Encryption encryption = new OpenControls.Wpf.DatabaseDialogs.Model.Encryption();
                if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MicrosoftSQLServer)
                {
                    MycoKeys.Library.PetaPocoAdapter.SQLServerDatabaseFactory.OpenDatabase(
                        out iDatabase,
                        openDatabaseViewModel.SQLServer_UseLocalServer,
                        openDatabaseViewModel.SelectedSqlServerInstance,
                        openDatabaseViewModel.SQLServer_IPAddress,
                        openDatabaseViewModel.SQLServer_Port,
                        openDatabaseViewModel.SQLServer_UseWindowsAuthentication,
                        openDatabaseViewModel.SQLServer_UserName,
                        encryption.Decrypt(openDatabaseViewModel.SQLServer_Password),
                        openDatabaseViewModel.SQLServer_DatabaseName);
                }
                else if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MySQL)
                {
                    MycoKeys.Library.PetaPocoAdapter.MySQLDatabaseFactory.OpenDatabase(
                        out iDatabase,
                        openDatabaseViewModel.MySQL_IPAddress,
                        openDatabaseViewModel.MySQL_Port,
                        openDatabaseViewModel.MySQL_UseWindowsAuthentication,
                        openDatabaseViewModel.MySQL_UserName,
                        encryption.Decrypt(openDatabaseViewModel.MySQL_Password),
                        openDatabaseViewModel.MySQL_DatabaseName);
                }
                else if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.PostGreSQL)
                {
                    MycoKeys.Library.PetaPocoAdapter.PostgreSQLServerDatabaseFactory.OpenDatabase(
                        out iDatabase,
                        openDatabaseViewModel.PostgreSQL_IPAddress,
                        openDatabaseViewModel.PostgreSQL_Port,
                        openDatabaseViewModel.PostgreSQL_UseWindowsAuthentication,
                        openDatabaseViewModel.PostgreSQL_UserName,
                        encryption.Decrypt(openDatabaseViewModel.PostgreSQL_Password),
                        openDatabaseViewModel.PostgreSQL_DatabaseName);
                }
                else if (openDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.SQLite)
                {
                    MycoKeys.Library.PetaPocoAdapter.SQLiterDatabaseFactory.OpenDatabase(
                        out iDatabase,
                        openDatabaseViewModel.SQLite_Filename);
                }
                else
                {
                    throw new Exception("Unsupported database type");
                }
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message);
                return false;
            }

            keyManager = MycoKeys.Library.PetaPocoAdapter.KeyManagerFactory.BuildKeyManager(iDatabase);

            return true;
        }

        private void _buttonOpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (OpenDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, out MycoKeys.Library.Database.KeyManager keyManager))
            {
                ShowKeysView(iDatabase, keyManager);
            }
        }

        private bool NewDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, out MycoKeys.Library.Database.KeyManager keyManager)
        {
            iDatabase = null;
            keyManager = null;

            OpenControls.Wpf.Serialisation.RegistryItemSerialiser registryItemSerialiser = new OpenControls.Wpf.Serialisation.RegistryItemSerialiser(_keyPath);
            OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration databaseConfiguration = new OpenControls.Wpf.DatabaseDialogs.Model.DatabaseConfiguration(registryItemSerialiser);
            if (registryItemSerialiser.OpenKey())
            {
                databaseConfiguration.Load();
            }

            OpenControls.Wpf.DatabaseDialogs.ViewModel.NewDatabaseViewModel newDatabaseViewModel = new OpenControls.Wpf.DatabaseDialogs.ViewModel.NewDatabaseViewModel(databaseConfiguration);
            OpenControls.Wpf.DatabaseDialogs.View.NewDatabaseView newDatabaseView =
                new OpenControls.Wpf.DatabaseDialogs.View.NewDatabaseView(new OpenControls.Wpf.DatabaseDialogs.Model.Encryption());
            newDatabaseView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newDatabaseView.Owner = this;
            newDatabaseView.DataContext = newDatabaseViewModel;
            if (newDatabaseView.ShowDialog() != true)
            {
                return false;
            }
            if (!registryItemSerialiser.IsOpen)
            {
                registryItemSerialiser.CreateKey();
            }
            databaseConfiguration.Save();
            registryItemSerialiser.Close();

            try
            {
                OpenControls.Wpf.DatabaseDialogs.Model.Encryption encryption = new OpenControls.Wpf.DatabaseDialogs.Model.Encryption();
                if (newDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MicrosoftSQLServer)
                {
                    MycoKeys.Library.PetaPocoAdapter.SQLServerDatabaseFactory.CreateDatabase(
                        out iDatabase,
                        newDatabaseViewModel.SelectedSqlServerInstance,
                        newDatabaseViewModel.SQLServer_UseWindowsAuthentication,
                        newDatabaseViewModel.SQLServer_UserName,
                        encryption.Decrypt(newDatabaseViewModel.SQLServer_Password),
                        newDatabaseViewModel.SQLServer_Folder,
                        newDatabaseViewModel.SQLServer_Filename);
                }
                else if (newDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.MySQL)
                {
                    MycoKeys.Library.PetaPocoAdapter.MySQLDatabaseFactory.CreateDatabase(
                        out iDatabase,
                        newDatabaseViewModel.MySQL_IPAddress,
                        newDatabaseViewModel.MySQL_Port,
                        newDatabaseViewModel.MySQL_UseWindowsAuthentication,
                        newDatabaseViewModel.MySQL_UserName,
                        encryption.Decrypt(newDatabaseViewModel.MySQL_Password),
                        newDatabaseViewModel.MySQL_DatabaseName);
                }
                else if (newDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.PostGreSQL)
                {
                    MycoKeys.Library.PetaPocoAdapter.MySQLDatabaseFactory.CreateDatabase(
                        out iDatabase,
                        newDatabaseViewModel.PostgreSQL_IPAddress,
                        newDatabaseViewModel.PostgreSQL_Port,
                        newDatabaseViewModel.PostgreSQL_UseWindowsAuthentication,
                        newDatabaseViewModel.PostgreSQL_UserName,
                        encryption.Decrypt(newDatabaseViewModel.PostgreSQL_Password),
                        newDatabaseViewModel.PostgreSQL_DatabaseName);
                }
                else if (newDatabaseViewModel.SelectedDatabaseProvider == OpenControls.Wpf.DatabaseDialogs.Model.DatabaseProvider.SQLite)
                {
                    MycoKeys.Library.PetaPocoAdapter.SQLiterDatabaseFactory.CreateDatabase(
                        out iDatabase,
                        newDatabaseViewModel.SQLite_Folder,
                        newDatabaseViewModel.SQLite_DatabaseName);
                }
                else
                {
                    throw new Exception("Unsupported database type");
                }

                keyManager = MycoKeys.Library.PetaPocoAdapter.KeyManagerFactory.BuildKeyManager(iDatabase);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.Message);
                return false;
            }

            return true;
        }

        private void _buttonNewDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (NewDatabase(out MycoKeys.Library.Database.IDatabase iDatabase, out MycoKeys.Library.Database.KeyManager keyManager))
            {
                ShowKeysView(iDatabase, keyManager);
            }
        }

        private void _buttonExportDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (!OpenDatabase(out MycoKeys.Library.Database.IDatabase iSourceDatabase, out MycoKeys.Library.Database.KeyManager sourceKeyManager))
            {
                if (iSourceDatabase != null)
                {
                    iSourceDatabase.CloseConnection();
                }
                return;
            }

            if (!NewDatabase(out MycoKeys.Library.Database.IDatabase iTargetDatabase, out MycoKeys.Library.Database.KeyManager targetKeyManager))
            {
                if (iTargetDatabase != null)
                {
                    iTargetDatabase.CloseConnection();
                }
                if (iSourceDatabase != null)
                {
                    iSourceDatabase.CloseConnection();
                }
                return;
            }

            MycoKeys.Library.Database.KeyManager.Export(sourceKeyManager, targetKeyManager);

            // Warning warning
            iSourceDatabase.CloseConnection();
            iTargetDatabase.CloseConnection();
        }

        private void _buttonImportDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (!OpenDatabase(out MycoKeys.Library.Database.IDatabase iSourceDatabase, out MycoKeys.Library.Database.KeyManager sourceKeyManager))
            {
                if (iSourceDatabase != null)
                {
                    iSourceDatabase.CloseConnection();
                }
                return;
            }

            if (!OpenDatabase(out MycoKeys.Library.Database.IDatabase iTargetDatabase, out MycoKeys.Library.Database.KeyManager targetKeyManager))
            {
                if (iTargetDatabase != null)
                {
                    iTargetDatabase.CloseConnection();
                }
                if (iSourceDatabase != null)
                {
                    iSourceDatabase.CloseConnection();
                }
                return;
            }

            MycoKeys.Library.Database.KeyManager.Export(sourceKeyManager, targetKeyManager);

            // Warning warning
            iSourceDatabase.CloseConnection();
            iTargetDatabase.CloseConnection();
        }

        public void Import(IKeyManager targetKeyManager)
        {
            if (!OpenDatabase(out MycoKeys.Library.Database.IDatabase iSourceDatabase, out MycoKeys.Library.Database.KeyManager sourceKeyManager))
            {
                if (iSourceDatabase != null)
                {
                    iSourceDatabase.CloseConnection();
                }
                return;
            }

            MycoKeys.Library.Database.KeyManager.Export(sourceKeyManager, targetKeyManager);
            iSourceDatabase.CloseConnection();
        }

        public void Export(IKeyManager sourceKeyManager)
        {
            if (!NewDatabase(out MycoKeys.Library.Database.IDatabase iTargetDatabase, out MycoKeys.Library.Database.KeyManager targetKeyManager))
            {
                if (iTargetDatabase != null)
                {
                    iTargetDatabase.CloseConnection();
                }
                return;
            }

            MycoKeys.Library.Database.KeyManager.Export(sourceKeyManager, targetKeyManager);
            iTargetDatabase.CloseConnection();
        }

        private void _buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
