using System.Diagnostics;
using System.Windows.Input;
using View.AppVM;
using ViewModel;

namespace View.Views
{
    public partial class ChampionFormPage : ContentPage
    {
        public ChampionFormVM ChampionFormVM => championFormVM;
        private readonly ChampionFormVM championFormVM;

        public MainAppVM MainAppVM => mainAppVM;
        private readonly MainAppVM mainAppVM;

        public ChampionFormPage(ChampionFormVM championFormVM, MainAppVM mainAppVM)
        {
            this.championFormVM = championFormVM;
            this.mainAppVM = mainAppVM;
            InitializeComponent();
            BindingContext = ChampionFormVM;
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/navigationpage#disable-the-back-button
        /// </summary>
        /// <returns>true</returns>
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void OnAddCharacteristicClicked(object sender, EventArgs e)
        {
            NewCharacteristicKey.Text = "";
            NewCharacteristicValue.Text = "";
        }

        private void OnAddSkillClicked(object sender, EventArgs e)
        {
            NewSkillName.Text = "";
            NewSkillType.SelectedItem = null;
            NewSkillDescription.Text = "";
        }

        public async void OnUploadIconButtonClicked(object sender, EventArgs e)
        {
            await UploadImage(ChampionFormVM.UpsertIconCommand);
        }

        public async void OnUploadImageButtonClicked(object sender, EventArgs e)
        {
            await UploadImage(ChampionFormVM.UpsertImageCommand);
        }

        private async Task UploadImage(ICommand command)
        {
            var imageBytes = await UploadImage();
            if (imageBytes != null)
            {
                await Dispatcher.DispatchAsync(() =>
                {
                    command.Execute(imageBytes);
                    return Task.CompletedTask;
                });
            }
        }

        public async static Task<byte[]?> UploadImage()
        {
            try
            {
                var pickedFile = await FilePicker.Default.PickAsync(PickOptions.Images);

                if (pickedFile != null)
                {
                    using var stream = await pickedFile.OpenReadAsync();
                    byte[] bytes = new byte[stream.Length];
                    await stream.ReadAsync(new Memory<byte>(bytes));
                    return bytes;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
