using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Manning.MyPhotoAlbum;


namespace Myphoto
{
    public partial class MainForm : Form
    {
        private PictureBox pbxPhoto;
        private ContextMenuStrip ctxMenuPhoto;
        private IContainer components;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuImage;
        private ToolStripMenuItem mnuImageScale;
        private ToolStripMenuItem mnuImageStretch;
        private ToolStripMenuItem mnuIamgeActual;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem mnuNext;
        private ToolStripMenuItem mnuPrevious;
        private ToolStripMenuItem mnuPhotoProperties;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem mnuFileNew;
        private ToolStripMenuItem mnuFileOpen;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem mnuFileSave;
        private ToolStripMenuItem mnuFileSaveAs;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem mnuFilePrint;
        private ToolStripMenuItem mnuFilePrintPreview;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem mnuFileExit;
        private ToolStripMenuItem mnuEdit;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem mnuEditCut;
        private ToolStripMenuItem mnuEditCopy;
        private ToolStripMenuItem mnuEditPaste;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem mnuEditAdd;
        private ToolStripMenuItem mnuEditRemove;
        private ToolStripMenuItem mnuView;
        private ToolStripMenuItem mnuHelp;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem mnuHelpAbout;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel sttInfo;
        private ToolStripStatusLabel sttImageSize;
        private ToolStripStatusLabel sttAlbumPos;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem mnuPixelData;
        private AlbumManager _manager;
        private AlbumManager Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                _manager = value;
            }
        }
        public MainForm()
        {
            InitializeComponent();
            NewAlbum();
        }

        private PixelDialog _dlgPixel = null;
        private PixelDialog PixelForm
        {
            get { return _dlgPixel; }
            set { _dlgPixel = value; }
        }
        private void NewAlbum()
        {
            if (Manager == null || SaveAndCloseAlbum())
            {
                Manager = new AlbumManager();
                DisplayAlbum();
            }
        }
        private void DisplayAlbum()
        {
            pbxPhoto.Image = Manager.CurrentImage;
            SetTitleBar();
            SetStatusStrip(null);
            Point p = pbxPhoto.PointToClient(MousePosition);
            UpdatePixelDialog(p.X, p.Y);
        }
        private void SetTitleBar()
        {
            Version ver = new Version(Application.ProductVersion);
            string name = Manager.FullName;

            Text = String.Format("{2}-MyPhotos {0:0}.{1:0}",

                                  ver.Major, ver.Minor,
                                  String.IsNullOrEmpty(name) ? "Untitled" : name);

        }
        private void Form1_Load(object sender, EventArgs e)
        { }



        private void mnuFileLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Photo";
            dlg.Filter = "jpg files (*.jpg)|*.jpg"
                         + "|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pbxPhoto.Image = new Bitmap(dlg.OpenFile());
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(
                              "Unable to load file: " + ex.Message);
                    pbxPhoto.Image = null;
                }
                SetStatusStrip(dlg.FileName);

            }
            dlg.Dispose();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuImage_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessImageClick(e);
        }
        private void ProcessImageClick(ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string enumVal = item.Tag as string;
            if (enumVal != null)
            {
                pbxPhoto.SizeMode = (PictureBoxSizeMode)Enum.Parse(typeof(PictureBoxSizeMode), enumVal);
            }
        }

        private void mnuImage_DropDownOpening(object sender, EventArgs e)
        {
            ProcessImageOpening(sender as ToolStripDropDownItem);
        }
        private void ProcessImageOpening(ToolStripDropDownItem parent)
        {
            if (parent != null)
            {
                string enumVal = pbxPhoto.SizeMode.ToString();
                foreach (ToolStripMenuItem item in parent.DropDownItems)
                {
                    item.Enabled = (pbxPhoto.Image != null);
                    item.Checked = item.Tag.Equals(enumVal);
                }
            }
        }
        private void SetStatusStrip(string path)
        {
            if (pbxPhoto.Image != null)
            {
                sttInfo.Text = Manager.Current.Filename;
                sttImageSize.Text = String.Format("{0:#}x{1:#}",
                                                  pbxPhoto.Image.Width,
                                                  pbxPhoto.Image.Height);
                sttAlbumPos.Text = String.Format("{0:0}/{1:0}", Manager.Index + 1, Manager.Album.Count);
            }
            else
            {
                sttInfo.Text = null;
                sttImageSize.Text = null;
                sttAlbumPos.Text = null;
            }
        }

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            NewAlbum();
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Album";
            dlg.Filter = "Album files (*.abm)|*.abm"
                         + "|All files (*.*)|*.*";
            dlg.InitialDirectory = AlbumManager.DefaultPath;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // TODO: save any existing album

                // Open: the new abum
                // TODO: handle invalid album file
                Manager = new AlbumManager(dlg.FileName);
                DisplayAlbum();

            }
            dlg.Dispose();
        }
        private void SaveAlbum(string name)
        {
            try
            {
                Manager.Save(name, true);
            }

            catch (AlbumStorageException aex)
            {
                string msg = String.Format("Unable to save album {0} ({1})\n\nDo you wish to save the album under a alternate name?",
                                           name, aex.Message);
                DialogResult result = MessageBox.Show(msg, "Unable to save",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Error,
                                                      MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                    SaveAsAlbum();
            }


        }

        private void SaveAlbum()
        {
            if (string.IsNullOrEmpty(Manager.FullName))
                SaveAsAlbum();
            else
            {
                // Save the album  under the existing name
                SaveAlbum(Manager.FullName);

            }
        }

        private void SaveAsAlbum()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = " Save Album";
            dlg.DefaultExt = "abm";
            dlg.Filter = "Album files (*.abm)|*.abm"
                         + "|All files (*.*)|*.*";
            dlg.InitialDirectory = AlbumManager.DefaultPath;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SaveAlbum(dlg.FileName);
                // Update title bar to include new name
                SetTitleBar();
            }
            dlg.Dispose();
        }
        private bool SaveAndCloseAlbum()
        {
            if (Manager.Album.HasChanged)
            {
                string msg;
                if (string.IsNullOrEmpty(Manager.FullName))
                    msg = "Do you wish to save your changes?";
                else
                    msg = String.Format("Do you wish to save your changes to \n" + Manager.FullName);

                DialogResult result = MessageBox.Show(this, msg, "Save Changes?",
                                                      MessageBoxButtons.YesNoCancel,
                                                      MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    SaveAlbum();
                else if (result == DialogResult.Cancel)
                    return false;
            }

            if (Manager.Album != null)
                Manager.Album.Dispose();

            Manager = new AlbumManager();
            SetTitleBar();
            return true;
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            SaveAlbum();
        }
        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsAlbum();

        }

        private void mnuEditAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Add Photos";
            dlg.Multiselect = true;
            dlg.Filter = " Image Files((JPEG, GIF, BMP, etc.)|"
            + "*.jpg;*.jpeg;*.gif;*.bmp;"
            + "*.tif;*.tiff;*.png|"
            + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
            + "GIF files (*.gif)|*.gif|"
            + "BMP files (*.bmp)|*.bmp|"
            + "TIFF files (*.tif;*.tiff)|*.tif;*.tiff|"
            + "PNG files (*.png)|*.png|"
            + "All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.CurrentDirectory;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = dlg.FileNames;

                int Index = 0;
                foreach (string s in files)
                {
                    Photograph photo = new Photograph(s);
                    Index = Manager.Album.IndexOf(photo);
                    if (Index < 0)
                        Manager.Album.Add(photo);
                    else
                        photo.Dispose();
                }
                Manager.Index = Manager.Album.Count - 1;
            }
            dlg.Dispose();
            DisplayAlbum();
        }

        private void munFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Album";
            dlg.Filter = "Album files (*.abm)|*.abm"
                         + "|All files (*.*)|*.*";
            dlg.InitialDirectory = AlbumManager.DefaultPath;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = dlg.FileName;
                if (!SaveAndCloseAlbum())
                    return;
                try
                {
                    //Open the new album

                    Manager = new AlbumManager(path);

                }
                catch (AlbumStorageException aex)
                {
                    string msg = String.Format("Unable to open album file {0}\n({1})",
                                               path, aex.Message);
                    MessageBox.Show(msg, "Unable to Open");
                    Manager = new AlbumManager();
                }
                Manager = new AlbumManager(dlg.FileName);
                DisplayAlbum();

            }
            dlg.Dispose();
        }

        private void mnuEditRemove_Click(object sender, EventArgs e)
        {
            if (Manager.Album.Count > 0)
            {
                Manager.Album.RemoveAt(Manager.Index);
                DisplayAlbum();
            }
        }

        private void mnuNext_Click(object sender, EventArgs e)
        {
            if (Manager.Index < Manager.Album.Count - 1)
            {
                Manager.Index++;
                DisplayAlbum();
            }
        }

        private void mnuPrevious_Click(object sender, EventArgs e)
        {
            if (Manager.Index > 0)
            {
                Manager.Index--;
                DisplayAlbum();
            }
        }

        private void ctxMenuPhoto_Opening(object sender, CancelEventArgs e)
        {
            //mnuNext.Enabled = (Manager.Index < Manager.Album.Count - 1);
            //mnuPrevious.Enabled = (Manager.Index > 0);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pbxPhoto = new System.Windows.Forms.PictureBox();
            this.ctxMenuPhoto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImageScale = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImageStretch = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIamgeActual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuNext = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPhotoProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPixelData = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilePrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sttInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.sttImageSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.sttAlbumPos = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPhoto)).BeginInit();
            this.ctxMenuPhoto.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxPhoto
            // 
            this.pbxPhoto.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbxPhoto.ContextMenuStrip = this.ctxMenuPhoto;
            this.pbxPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxPhoto.Location = new System.Drawing.Point(0, 24);
            this.pbxPhoto.Name = "pbxPhoto";
            this.pbxPhoto.Size = new System.Drawing.Size(355, 274);
            this.pbxPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxPhoto.TabIndex = 1;
            this.pbxPhoto.TabStop = false;
            this.pbxPhoto.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbxPhoto_MouseMove);
            // 
            // ctxMenuPhoto
            // 
            this.ctxMenuPhoto.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuImage,
            this.toolStripSeparator1,
            this.mnuNext,
            this.mnuPrevious,
            this.mnuPhotoProperties,
            this.toolStripSeparator7,
            this.mnuPixelData});
            this.ctxMenuPhoto.Name = "ctxMenuPhoto";
            this.ctxMenuPhoto.Size = new System.Drawing.Size(193, 126);
            this.ctxMenuPhoto.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuPhoto_Opening);
            // 
            // mnuImage
            // 
            this.mnuImage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuImageScale,
            this.mnuImageStretch,
            this.mnuIamgeActual});
            this.mnuImage.Name = "mnuImage";
            this.mnuImage.Size = new System.Drawing.Size(192, 22);
            this.mnuImage.Text = "&Image";
            this.mnuImage.DropDownOpening += new System.EventHandler(this.mnuImage_DropDownOpening);
            this.mnuImage.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuImage_DropDownItemClicked);
            // 
            // mnuImageScale
            // 
            this.mnuImageScale.Checked = true;
            this.mnuImageScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuImageScale.Name = "mnuImageScale";
            this.mnuImageScale.Size = new System.Drawing.Size(141, 22);
            this.mnuImageScale.Tag = "Zoom";
            this.mnuImageScale.Text = "&Sacle to Fit";
            // 
            // mnuImageStretch
            // 
            this.mnuImageStretch.Name = "mnuImageStretch";
            this.mnuImageStretch.Size = new System.Drawing.Size(141, 22);
            this.mnuImageStretch.Tag = "StretchImage";
            this.mnuImageStretch.Text = "S&tretch to Fit";
            // 
            // mnuIamgeActual
            // 
            this.mnuIamgeActual.Name = "mnuIamgeActual";
            this.mnuIamgeActual.Size = new System.Drawing.Size(141, 22);
            this.mnuIamgeActual.Tag = "Normal";
            this.mnuIamgeActual.Text = "&Actual Size";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
            // 
            // mnuNext
            // 
            this.mnuNext.Name = "mnuNext";
            this.mnuNext.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.mnuNext.Size = new System.Drawing.Size(192, 22);
            this.mnuNext.Text = "&Next";
            // 
            // mnuPrevious
            // 
            this.mnuPrevious.Name = "mnuPrevious";
            this.mnuPrevious.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.mnuPrevious.Size = new System.Drawing.Size(192, 22);
            this.mnuPrevious.Text = "&Previous";
            // 
            // mnuPhotoProperties
            // 
            this.mnuPhotoProperties.Name = "mnuPhotoProperties";
            this.mnuPhotoProperties.Size = new System.Drawing.Size(192, 22);
            this.mnuPhotoProperties.Text = "Phot&o Properties...";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(189, 6);
            // 
            // mnuPixelData
            // 
            this.mnuPixelData.Name = "mnuPixelData";
            this.mnuPixelData.Size = new System.Drawing.Size(192, 22);
            this.mnuPixelData.Text = "Pi&xel Data...";
            this.mnuPixelData.Click += new System.EventHandler(this.mnuPixelData_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuEdit,
            this.mnuView,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(355, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileNew,
            this.mnuFileOpen,
            this.toolStripSeparator,
            this.mnuFileSave,
            this.mnuFileSaveAs,
            this.toolStripSeparator2,
            this.mnuFilePrint,
            this.mnuFilePrintPreview,
            this.toolStripSeparator3,
            this.mnuFileExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileNew.Image")));
            this.mnuFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFileNew.Name = "mnuFileNew";
            this.mnuFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuFileNew.Size = new System.Drawing.Size(146, 22);
            this.mnuFileNew.Text = "&New";
            this.mnuFileNew.Click += new System.EventHandler(this.mnuFileNew_Click);
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileOpen.Image")));
            this.mnuFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuFileOpen.Size = new System.Drawing.Size(146, 22);
            this.mnuFileOpen.Text = "&Open";
            this.mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileSave.Image")));
            this.mnuFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFileSave.Name = "mnuFileSave";
            this.mnuFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuFileSave.Size = new System.Drawing.Size(146, 22);
            this.mnuFileSave.Text = "&Save";
            this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
            // 
            // mnuFileSaveAs
            // 
            this.mnuFileSaveAs.Name = "mnuFileSaveAs";
            this.mnuFileSaveAs.Size = new System.Drawing.Size(146, 22);
            this.mnuFileSaveAs.Text = "Save &As";
            this.mnuFileSaveAs.Click += new System.EventHandler(this.mnuFileSaveAs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // mnuFilePrint
            // 
            this.mnuFilePrint.Image = ((System.Drawing.Image)(resources.GetObject("mnuFilePrint.Image")));
            this.mnuFilePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFilePrint.Name = "mnuFilePrint";
            this.mnuFilePrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mnuFilePrint.Size = new System.Drawing.Size(146, 22);
            this.mnuFilePrint.Text = "&Print";
            // 
            // mnuFilePrintPreview
            // 
            this.mnuFilePrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("mnuFilePrintPreview.Image")));
            this.mnuFilePrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFilePrintPreview.Name = "mnuFilePrintPreview";
            this.mnuFilePrintPreview.Size = new System.Drawing.Size(146, 22);
            this.mnuFilePrintPreview.Text = "Print Pre&view";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(143, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(146, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator4,
            this.mnuEditCut,
            this.mnuEditCopy,
            this.mnuEditPaste,
            this.toolStripSeparator5,
            this.mnuEditAdd,
            this.mnuEditRemove});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(39, 20);
            this.mnuEdit.Text = "&Edit";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(187, 6);
            // 
            // mnuEditCut
            // 
            this.mnuEditCut.Image = ((System.Drawing.Image)(resources.GetObject("mnuEditCut.Image")));
            this.mnuEditCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuEditCut.Name = "mnuEditCut";
            this.mnuEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mnuEditCut.Size = new System.Drawing.Size(190, 22);
            this.mnuEditCut.Text = "Cu&t";
            // 
            // mnuEditCopy
            // 
            this.mnuEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("mnuEditCopy.Image")));
            this.mnuEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuEditCopy.Name = "mnuEditCopy";
            this.mnuEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuEditCopy.Size = new System.Drawing.Size(190, 22);
            this.mnuEditCopy.Text = "&Copy";
            // 
            // mnuEditPaste
            // 
            this.mnuEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("mnuEditPaste.Image")));
            this.mnuEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuEditPaste.Name = "mnuEditPaste";
            this.mnuEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.mnuEditPaste.Size = new System.Drawing.Size(190, 22);
            this.mnuEditPaste.Text = "&Paste";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(187, 6);
            // 
            // mnuEditAdd
            // 
            this.mnuEditAdd.Name = "mnuEditAdd";
            this.mnuEditAdd.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.mnuEditAdd.Size = new System.Drawing.Size(190, 22);
            this.mnuEditAdd.Text = "Ad&d";
            this.mnuEditAdd.Click += new System.EventHandler(this.mnuEditAdd_Click);
            // 
            // mnuEditRemove
            // 
            this.mnuEditRemove.Name = "mnuEditRemove";
            this.mnuEditRemove.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.mnuEditRemove.Size = new System.Drawing.Size(190, 22);
            this.mnuEditRemove.Text = "Remo&ve";
            this.mnuEditRemove.Click += new System.EventHandler(this.mnuEditRemove_Click);
            // 
            // mnuView
            // 
            this.mnuView.DropDown = this.ctxMenuPhoto;
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(44, 20);
            this.mnuView.Text = "&View";
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator6,
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(113, 6);
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(116, 22);
            this.mnuHelpAbout.Text = "&About...";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sttInfo,
            this.sttImageSize,
            this.sttAlbumPos});
            this.statusStrip1.Location = new System.Drawing.Point(0, 274);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(355, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sttInfo
            // 
            this.sttInfo.AutoSize = false;
            this.sttInfo.Name = "sttInfo";
            this.sttInfo.Size = new System.Drawing.Size(264, 19);
            this.sttInfo.Spring = true;
            this.sttInfo.Text = "Desc";
            this.sttInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sttImageSize
            // 
            this.sttImageSize.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sttImageSize.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.sttImageSize.Name = "sttImageSize";
            this.sttImageSize.Size = new System.Drawing.Size(42, 19);
            this.sttImageSize.Text = "W x H";
            // 
            // sttAlbumPos
            // 
            this.sttAlbumPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sttAlbumPos.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.sttAlbumPos.Name = "sttAlbumPos";
            this.sttAlbumPos.Size = new System.Drawing.Size(34, 19);
            this.sttAlbumPos.Text = "1 / 1";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(355, 298);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pbxPhoto);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MyPhotos";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxPhoto)).EndInit();
            this.ctxMenuPhoto.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (SaveAndCloseAlbum() == false)
                e.Cancel = true;
            else
                e.Cancel = false;
            base.OnFormClosing(e);
        }

        private void mnuPixelData_Click(object sender, EventArgs e)
        {

            if (PixelForm == null || PixelForm.IsDisposed)
            {
                PixelForm = new PixelDialog();
                PixelForm.Owner = this;
            }

            PixelForm.Show();
            Point p = pbxPhoto.PointToClient(MousePosition);
            UpdatePixelDialog(p.X, p.Y);
        }

        private void UpdatePixelDialog(int X, int Y)
        {
            if (PixelForm != null && PixelForm.Visible)
            {
                Bitmap bmp = Manager.CurrentImage;
                PixelForm.Text = (Manager.Current == null) ? "Pixel Data" : Manager.Current.Caption;

                if (bmp == null || !pbxPhoto.DisplayRectangle.Contains(X, Y))
                    PixelForm.ClearPixelData();
                else
                    PixelForm.UpdatePixelData(X, Y, bmp, pbxPhoto.DisplayRectangle,
                                              new Rectangle(0, 0, bmp.Width, bmp.Height),
                                              pbxPhoto.SizeMode);
            }
        }

        

        private void pbxPhoto_MouseMove(object sender, MouseEventArgs e)
        {
            UpdatePixelDialog(e.X, e.Y);
        }
    }
    }