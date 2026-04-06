namespace SkillTrackerFront.Components.Pages
{
    public partial class LoaderSpin
    {
        bool Visible = false;

        protected override void OnInitialized()
        {
            Loader.OnShow += () => { Visible = true; StateHasChanged(); };
            Loader.OnHide += () => { Visible = false; StateHasChanged(); };
        }
    }
}
