using System.IO;
using System.Windows;
using Crocoteka.Tools;

namespace Crocoteka.Dialogs;

/// <summary>
/// Класс окна настроек базы данных.
/// </summary>
public partial class DatabaseDialog : Window
{
    private bool DbNameChanged =>
        !DbNameTextBox.Text.Equals(App.DbName, StringComparison.CurrentCultureIgnoreCase);

    public DatabaseDialog()
    {
        InitializeComponent();
#if DEBUG
        DbNameTextBox.Text = Properties.Settings.Default.DebugDbName;
#else
        DbNameTextBox.Text = Properties.Settings.Default.DbName;
#endif
        CheckDbNameChanged();
    }

    private void CheckDbNameChanged()
    {
        DbChangedStackPanel.Visibility = DbNameChanged ? Visibility.Visible : Visibility.Collapsed;
        DbNotChangedStackPanel.Visibility = DbNameChanged ? Visibility.Collapsed : Visibility.Visible;
        DbShrinkButton.IsEnabled = !DbNameChanged;
    }

    private void DbShrinkButton_Click(object sender, RoutedEventArgs e)
    {
        if (!App.ConfirmAction("Сжать базу данных библиотеки?", Title))
            return;
        var path = Path.GetDirectoryName(App.DbName) ?? "";
        var name = Path.GetFileNameWithoutExtension(App.DbName);
        var ext = Path.GetExtension(App.DbName);
        var filename = Path.Combine(path, name + "-backup" + ext);
        try { File.Delete(filename); }
        catch { }
        Db.Shrink();
        MessageBox.Show("Сжатие базы данных библиотеки завершено.", Title);
    }

    private void DbNameButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = App.PickDatabaseDialog;
        if (dialog.ShowDialog() != true)
            return;
        var dbName = Db.EnsureDbExtension(dialog.FileName);
        if (!Db.ValidateDb(dbName))
        {
            MessageBox.Show("Файл не является базой данных Крокотеки или повреждён.", Title);
            return;
        }
        DbNameTextBox.Text = dbName;
        CheckDbNameChanged();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (DbNameChanged)
        {
#if DEBUG
            Properties.Settings.Default.DebugDbName = DbNameTextBox.Text;
#else
            Properties.Settings.Default.DbName = DbNameTextBox.Text;
#endif
            var newDb = !File.Exists(DbNameTextBox.Text);
            using var db = Db.GetDatabase(DbNameTextBox.Text);
            if (newDb)
                Db.InitializeCollections(db);
            Properties.Settings.Default.Save();
            DialogResult = true;
        }
        else
            DialogResult = false;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
