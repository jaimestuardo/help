﻿namespace TimeApp;

public partial class App : Application
{
    public static string UserEmail { get; set; }
    public static string UserFullName { get; set; }

    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	public static async Task SetUserInformation()
	{
        UserEmail = await Helpers.LoginData.GetEmail();
        UserFullName = await Helpers.LoginData.GetFullName();
    }

    public static void ClearUserInformation()
    {
        Helpers.LoginData.Clear();
        UserEmail = UserFullName = string.Empty;
    }
}
