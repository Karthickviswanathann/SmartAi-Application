using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SkillTrackerFront.Components.DtoModels;
using SkillTrackerFront.Services;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;

namespace SkillTrackerFront.Components.Pages
{
    public partial class DashBoard
    {
        private string SearchTerm = "";
        private string SelectedSize = "10px";
        private string FontSize = "";
        private string FontFamily = "";
        private string NoteTitle = "";
        private string NoteContent = "";
        private string SelectedWorkspace = "General";
        private string SelectedFont = "Inter";
        private string selectedAccent = "#3b82f6";
        private bool isDarkMode;
        private bool isPinned;
        private bool isArchieve;
        private bool isOpenDownMod;
        private string accentColor;
        private string Element;
        private string theme;
        private string token;
        private string countOfWords;
        private string Username;
        bool showDeleteModal = false;
        bool isAppearanceOpen = false;
        Note deletedNotes;
        private bool isWPModOpen { get; set; }
        private bool isfontModOpen { get; set; }
        private bool isfontfamilyModOpen { get; set; }

        List <NotesActivityDto> Lz_GetNotes;

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
        public List<string> LZ_Workspaces = new()
        {
            "General","Work","Personal"
        };
        public List<string> AvailableFontSizes =
        Enumerable.Range(10, 15).Select(i => $"{i}px").ToList();

        private List<string> accentColors = new() { "#3b82f6", "#10b981", "#f59e0b", "#ef4444", "#8b5cf6", "#ec4899", "#06b6d4" };

        private List<Note> notes = new List<Note>();
        private List<Note> Lz_notes = new List<Note>();
        private List<Note> Lz_notesData = new List<Note>();
        private string eleme { get; set; }
        private string them { get; set; }

        public class BehaviourDto
        {
            public string? ThemeColor { get; set; }
            public string? ElementColor { get; set; }
        }

   
        protected async override Task OnInitializedAsync()
        {
            try
            {
                notes.Clear();
                Lz_notes.Clear();
                Lz_notesData.Clear();
                token = await JS.InvokeAsync<string>("sessionStorage.getItem", "authToken");
                Username = await JS.InvokeAsync<string>("sessionStorage.getItem", "Username");

                var res1 = await Flow.GetBehaviour(token);

                if(res1.Data != null)
                {
                    var json = JsonConvert.DeserializeObject<BehaviourDto>(res1.Data.ToString());


                    theme = string.IsNullOrEmpty(json.ThemeColor) ? "false" : json.ThemeColor;
                    Element = string.IsNullOrEmpty(json.ElementColor) ? "var(--accent-color)" : json.ElementColor;

                    await JS.InvokeVoidAsync("sessionStorage.setItem", "ThemeColor", theme);
                    await JS.InvokeVoidAsync("sessionStorage.setItem", "Elementcolor", Element);



                     eleme = await JS.InvokeAsync<string>("sessionStorage.getItem", "Elementcolor");
                     them = await JS.InvokeAsync<string>("sessionStorage.getItem", "ThemeColor");


                    await JS.InvokeVoidAsync("applyAccentColor", eleme);

                    isDarkMode = Convert.ToBoolean(them);

                    await JS.InvokeVoidAsync("setDarkMode", isDarkMode);

                }

                
                var res2 = await Flow.GetNotes(token);
                if (res2.data != null)
                {
                    Lz_GetNotes = res2.data.ToList();
                    foreach (var note in Lz_GetNotes) {

                        Lz_notes.Add(new Note{
                            Id=note.Id,
                            Title=note.Title,
                            NotesText=note.NotesText,
                            IsPinned=note.isPinned,
                            IsUrcheive=note.isUrcheive
                        });
                    
                    }

                }
                var json1 = JsonConvert.SerializeObject(Lz_notes.ToList());

                await JS.InvokeVoidAsync("sessionStorage.setItem", "notes", json1);

                var getJson=await JS.InvokeAsync<string>("sessionStorage.getItem", "notes");

                if (!string.IsNullOrEmpty(getJson))
                {
                    Lz_notesData = JsonConvert.DeserializeObject<List<Note>>(getJson);
                    notes = Lz_notesData
       .Where(x => x.IsUrcheive == false)   // filter first
       .OrderByDescending(n => n.IsPinned)  // then sort
       .ToList();
                    StateHasChanged();
                }


            }
            catch (Exception ex)
            {
                ShowMsg("Error", ex.Message, 3000);
            }

            isPinned = false;
        }


    

        private async Task ChangeAccentColor(ChangeEventArgs e)
        {
            accentColor = e.Value.ToString();

            await JS.InvokeVoidAsync("applyAccentColor", accentColor);

            await JS.InvokeVoidAsync("sessionStorage.setItem", "Elementcolor", accentColor);

            //await ThemeService.SaveAccentColorAsync(UserId, accentColor);
            var color = accentColor.ToString().Replace("#", "%23");
            var res = await Flow.PostBehaviour("", color, token);
            StateHasChanged();
        }
        private async Task ChangeFontSize(ChangeEventArgs e)
        {

            if(selectedNote != null)
            {
                FontSize = e.Value.ToString();

                StateHasChanged();
            }

            else
            {
                ShowMsg("Error","Select Or Create Notes are Required",3000);
                return;
            }
  
        }

        //private void AutoSave()
        //{
        //    autosaveTimer?.Stop();
        //    autosaveTimer = new System.Timers.Timer(30000); 
        //    autosaveTimer.Elapsed += async (_, _) => await SaveNotes();
        //    autosaveTimer.AutoReset = false;
        //    autosaveTimer.Start();
        //}
        private async void ApplyFont(ChangeEventArgs e)
        {

            if (selectedNote != null)
            {
                SelectedFont = e.Value.ToString();

                StateHasChanged();
            }

            else
            {
                ShowMsg("Error", "Select Or Create Notes are Required", 3000);
                return;
            }
        }

        void ToggleAppearance()
        {
            isAppearanceOpen = !isAppearanceOpen;
        }

        private async Task ToggleTheme()
        {
            //await Task.Delay(3000);
            isDarkMode = !isDarkMode;
            await JS.InvokeVoidAsync("setDarkMode", isDarkMode);


            await JS.InvokeVoidAsync("sessionStorage.setItem", "ThemeColor", isDarkMode);

            var res = await Flow.PostBehaviour(isDarkMode.ToString(),null, token);
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

        private async void TogglePin()
        {
            try
            {
                if (selectedNote == null)
                    return;
                selectedNote.IsPinned = !selectedNote.IsPinned;

                if (selectedNote.Id != 0)
                {
                    var res = await Flow.PostNotesActivity(Convert.ToString(selectedNote.IsPinned),"",selectedNote.Id,token);

                    if (res.respCode == "200")
                    {
                        StateHasChanged();
                        await OnInitializedAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Error",ex.Message,3000);
            }
            
        }


        private async void UrcheiveNotes()
        {
            try
            {
                if (selectedNote == null)
                    return;
                selectedNote.IsUrcheive = !selectedNote.IsUrcheive;



                if (selectedNote.Id != 0)
                {
                    var res = await Flow.PostNotesActivity("",Convert.ToString(selectedNote.IsUrcheive),selectedNote.Id, token);


                    if (selectedNote.IsUrcheive == true)
                    {
                        selectedNote = null;
                    }

                    StateHasChanged();
                    await OnInitializedAsync();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Error", ex.Message, 3000);
            }

        }

        private async Task GetOpenNotes()
        {
            try
            {
                notes = Lz_notesData
                .Where(x => x.IsUrcheive == false)
                .OrderByDescending(n => n.IsPinned)
                .ToList();

                StateHasChanged();

            }
            catch (Exception ex)
            {
                ShowMsg("Error", ex.Message, 3000);
            }
        }

        private async Task GetUrcheiveNotes()
        {
            try
            {
                notes = Lz_notesData
                .Where(x => x.IsUrcheive == true)   
                .OrderByDescending(n => n.IsPinned)  
                .ToList();

                StateHasChanged();

            }
            catch (Exception ex)
            {
                ShowMsg("Error", ex.Message, 3000);
            }
        }


        private async Task CopyNoteText()
        {
            if (selectedNote.NotesText != null)
            {
                await JS.InvokeVoidAsync("copyToClipboard", selectedNote.NotesText);
            }
        }
        private async Task downloadNotesTXT() {
            if(selectedNote != null)
            {
                await JS.InvokeVoidAsync("downloadFile", "note.txt", selectedNote.NotesText);
            }
           
        }
        private async Task downloadNotesPDF() {
            if (selectedNote != null)
            {
                await JS.InvokeVoidAsync("downloadPDF", selectedNote.NotesText);
            }

        }
    
        private async Task OpenDownloadModel()
        {
            isOpenDownMod = !isOpenDownMod;
            
        }

        public async Task SaveNotes()
        {
            if (string.IsNullOrEmpty(selectedNote.NotesText))
                return;

            else
            {
                Loader.Show();
                var selectN = new AddNotesDto
                {
                    Id=selectedNote.Id,
                    Title=selectedNote.Title,
                    NotesText=selectedNote.NotesText,
                    Workspace=SelectedWorkspace
                };

                var res = await Flow.PostNotes(selectN, token);
               
              

                ShowMsg("Success", "Notes Added Succesfully", 3000);
                selectedNote = null;
                StateHasChanged();
                OnInitializedAsync();
                Loader.Hide();

                return;

            }

        
        }




        public void ConfirmDeleteNote()
        {
            deletedNotes = null;
            showDeleteModal = true;

            if (deletedNotes == null) {
                deletedNotes = selectedNote;
            }

        }

        public async Task DeleteNote()
        {
            //await JS.InvokeVoidAsync("stopEvent", e.ClientX, e.ClientY);

            notes.Remove(deletedNotes);

            if (selectedNote == deletedNotes)
                selectedNote = null;

            if (deletedNotes.Id != 0)
            {
                var res = await Flow.DeleteNotes(deletedNotes.Id, token);
            }
            showDeleteModal = false;

            StateHasChanged();
            OnInitializedAsync();

        }
        public class Note
        {
            public int Id { get; set; }
            public string Title { get; set; } = "";
            public string NotesText { get; set; } = "";
            public string? Workspace { get; set; }
            public bool IsSelected { get; set; } = false;
            public bool? IsPinned { get; set; }
            public bool? IsUrcheive { get; set; }
        }



        private Note? selectedNote;

        private void CreateNewNote()
        {
            try
            {
                var newNote = new Note
                {
                    Title = "Untitled Note",
                    NotesText = ""
                };      
                var isId = notes.FirstOrDefault(x => x.Id == newNote.Id);

                if (isId==null) {
                    notes.Add(newNote);
                    selectedNote = newNote;
                }
                else
                {
                    return;
                }
         
            }
            catch (Exception ex)
            {

                ShowMsg("Error", ex.Message, 3000);
                return;
            }

           
        }

        private void SelectNote(Note note)
        {
            notes.ForEach(n => n.IsSelected = false);
            note.IsSelected = true;
            selectedNote = note;
        }

        private void SelectPin(Note note)
        {
            notes.ForEach(n => n.IsPinned = false);
            note.IsPinned = true;
            selectedNote = note;
        }

        public void ShowMsg(string Type, string Msg, int duration)
        {
            JS.InvokeVoidAsync("barmessage", Type, Msg, duration);
        }
        private void CountWords(ChangeEventArgs e)
        {
            string text = e.Value?.ToString() ?? "";
            countOfWords = text.Count(char.IsLetter).ToString();
            StateHasChanged();
        }

        private void ToggleDropdown(string type)
        {
            if (type == "Workspace")
            {
                isWPModOpen = !isWPModOpen;
            }
            else if(type=="fontsize")
            {
                isfontModOpen = !isfontModOpen;
                isfontfamilyModOpen = false;
            }
            else if (type == "fontfamily")
            {
                isfontfamilyModOpen = !isfontfamilyModOpen;
                isfontModOpen = false;
            }
        }

        private void SelectItem(string value,string type)
        {

            if (type == "Workspace")
            {
                SelectedWorkspace = value;
                isWPModOpen = false;
                
            }
            else if (type == "fontsize")
            {
                SelectedSize =value;
                isfontModOpen = false;
            }
            else if (type == "fontfamily")
            {
                SelectedFont = value;
                isfontfamilyModOpen = false;
            }

        }




    }
}
