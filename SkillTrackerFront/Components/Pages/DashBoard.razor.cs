using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SkillTrackerFront.Components.DtoModels;
using SkillTrackerFront.Services;
using static System.Net.WebRequestMethods;

namespace SkillTrackerFront.Components.Pages
{
    public partial class DashBoard
    {
        private string SearchTerm = "";
        private string SelectedSize = "";
        private string NoteTitle = "";
        private string NoteContent = "";
        private string SelectedWorkspace = "General";
        private string SelectedFont = "Inter";
        private string selectedAccent = "#3b82f6";
        private bool isDarkMode;
        private string accentColor;
        private string Element;
        private string theme;
        private string token;

        private System.Timers.Timer? autosaveTimer;

        public List<string> AvailableFonts = new()
            {
                "Inter",
                "Roboto",
                "Open Sans",
                "Poppins",
                "Lato",
                "Montserrat",
                "Nunito",
                "Georgia"
            };

        private List<string> accentColors = new() { "#3b82f6", "#10b981", "#f59e0b", "#ef4444", "#8b5cf6", "#ec4899", "#06b6d4" };

        private List<Note> notes = new List<Note>();

        public class BehaviourDto
        {
            public string? ThemeColor { get; set; }
            public string? ElementColor { get; set; }
        }

   
        protected async override Task OnInitializedAsync()
        {
            try
            {
                token= await JS.InvokeAsync<string>("sessionStorage.getItem", "authToken");

                var res1 = await Flow.GetBehaviour(token);

                if(res1.Data != null)
                {
                    var json = JsonConvert.DeserializeObject<BehaviourDto>(res1.Data.ToString());

                    theme = json.ThemeColor;
                    Element = json.ElementColor;

                    if (theme== "IsDarkMode")
                        await JS.InvokeVoidAsync("addDarkMode");

                    await JS.InvokeVoidAsync("applyAccentColor", Element);
                }


                var res2 = await Flow.GetNotes(token);
                if (res2.Data != null)
                {
                    var resNotes = JsonConvert.DeserializeObject<List<AddNotesDto>>(res2.Data.ToString());

                    foreach (var note in resNotes) {

                        notes.Add(new Note{
                            Id=note.Id,
                            Title=note.Title,
                            NotesText=note.NotesText
                        });
                    
                    }
                }

                StateHasChanged();

            }
            catch (Exception ex)
            {
                ShowMsg("Error", ex.Message, 3000);
            }


        }


        private async Task ChangeAccentColor(ChangeEventArgs e)
        {
            accentColor = e.Value.ToString();

            await JS.InvokeVoidAsync("applyAccentColor", accentColor);

            //await ThemeService.SaveAccentColorAsync(UserId, accentColor);

            var res = await Flow.PostBehaviour(accentColor,null,token);
            StateHasChanged();
        }



        private void AutoSave()
        {
            autosaveTimer?.Stop();
            autosaveTimer = new System.Timers.Timer(30000); 
            autosaveTimer.Elapsed += async (_, _) => await SaveNotes();
            autosaveTimer.AutoReset = false;
            autosaveTimer.Start();
        }
        private async void ApplyFont(ChangeEventArgs e)
        {
            SelectedFont = e.Value.ToString();

            await JS.InvokeVoidAsync("applyFontToBody", SelectedFont);
        }
        private async void ToggleTheme()
        {
            isDarkMode = !isDarkMode;

            if (isDarkMode)
                await JS.InvokeVoidAsync("addDarkMode");
            else
                await JS.InvokeVoidAsync("removeDarkMode");
        }
    private void SelectAccent(string color) => selectedAccent = color;
        private void SignOut() {
            try
            {
                var state = (CustAuthStateProvider)stateprovider;
                state.NotifyUserLogout();
                StateHasChanged();
            }
            catch (Exception ex)
            {

                ShowMsg("Error", ex.Message, 3000);
            }
        
        
        }

        public async Task SaveNotes()
        {
            if (string.IsNullOrEmpty(selectedNote.NotesText))
                return;

            else
            {
                var selectN = new AddNotesDto
                {
                    Id=selectedNote.Id,
                    Title=selectedNote.Title,
                    NotesText=selectedNote.NotesText
                };

                var res = await Flow.PostNotes(selectN, token);
            }

        
        }


        public async Task DeleteNote(Note note, MouseEventArgs e)
        {
            //await JS.InvokeVoidAsync("stopEvent", e.ClientX, e.ClientY);

            notes.Remove(note);

            if (selectedNote == note)
                selectedNote = null;

            StateHasChanged();
        }
        public class Note
        {
            public int Id { get; set; }
            public string Title { get; set; } = "";
            public string NotesText { get; set; } = "";
            public bool IsSelected { get; set; }
        }



        private Note? selectedNote;

        private void CreateNewNote()
        {
            var newNote = new Note
            {
                Title = "Untitled Note",
                NotesText = ""
            };

            notes.ForEach(n => n.IsSelected = false);
            newNote.IsSelected = true;

            notes.Add(newNote);
            selectedNote = newNote;
            AutoSave();
        }

        private void SelectNote(Note note)
        {
            notes.ForEach(n => n.IsSelected = false);
            note.IsSelected = true;
            selectedNote = note;
        }


        public void ShowMsg(string Type, string Msg, int duration)
        {
            JS.InvokeVoidAsync("barmessage", Type, Msg, duration);
        }



    }
}
