using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using NAudio.Wave;
using LightBuzz.Vitruvius;
using System.Reactive.Linq;
using System.Linq;
using Microsoft.Gestures;
using Microsoft.Gestures.Endpoint;
using Microsoft.Gestures.Skeleton;
using Microsoft.Gestures.Stock.Gestures;
using Microsoft.Gestures.Stock.HandPoses;

using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

using System.Drawing.Imaging;
using Microsoft.Kinect.Face;
using System.Windows.Shapes;
using System.Configuration;

namespace MusicMaker
{

    public partial class frmMusicMaker : Form
    {

        private KinectSensor kinectSensor = null;
        // Acquires body frame data.
        private BodyFrameSource _bodySource = null;

        // Reads body frame data.
        private BodyFrameReader _bodyReader = null;

        // Acquires HD face data.
        private HighDefinitionFaceFrameSource _faceSource = null;

        // Reads HD face data.
        private HighDefinitionFaceFrameReader _faceReader = null;

        // Required to access the face vertices.
        private FaceAlignment _faceAlignment = null;

        // Required to access the face model points.
        private FaceModel _faceModel = null;

        // Used to display 1,000 points on screen.
        private List<Ellipse> _points = new List<Ellipse>();

        private MultiSourceFrameReader multiSourceFrameReader = null;

        private Bitmap colorImageBitmap = null;

        private byte[] colorImagePixelData = null;

        private ushort[] irImagePixelData = null;
        private ushort[] irImagePixelDataOld = null;

        private ushort[] irLEImagePixelData = null;

        long lastTime = 0;
        uint pulses = 0;
        Queue<float> hueValues = new Queue<float>();
        Queue<float> irValues = new Queue<float>();


        private int bodyCount = 0;

        private Body[] bodies = null;

        private int bodyToTrack = -1;

        ImageType imageType = ImageType.Color;
        Mode _mode = Mode.Depth;
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        public GesturesServiceEndpoint _gesturesService;
        private Gesture countGesture;

        public Gesture startGesture { get; private set; }
        public Gesture _pianothumb { get; private set; }

        private Gesture _startPose;
        private Gesture _start2Pose;
        private Gesture _swipeleft;
        private Gesture _nextChannelGesture;
        private Gesture _startPose00;
        private Gesture _start22Pose;
        private Gesture _startPose2;
        private Gesture _startPose0;
        private Gesture _rotateGesture;

        public Gesture _pianoindex { get; private set; }
        public Gesture _startGesture { get; private set; }
        public Gesture _pianopinky { get; private set; }
        public Gesture _pianoring { get; private set; }
        public Gesture _pianomiddle { get; private set; }

        private Gesture _pianoindexGesture;
        private int _rotateTimes = 0;
        ColorFrameReader colorFrameReader; //camera visual
        BodyFrameReader bodyFrameReader; //body sensor
        WriteableBitmap colorBitmap;

        public frmMusicMaker()
        {

            InitializeComponent();

            initCmbTranspose();

            GestureinitAsync();
            init();
        }


        DrawingGroup bodyHighlight;
        Body[] bodys;
        WaveOut waveOut;
        SineWaveOscillator osc;
        double freqReference;
        double freqRatio;
        double pitchReference;
        double currentPitch;
        double baseVol;
        float freqMult;
        //IWaveSource _source;    // Assuming that is already available
        //DmoChorusEffect _chor;
        //DmoWavesReverbEffect _rev;

        // DmoEchoEffect _echo;
        bool _displayBody = true;

        private const int totalSoundFontFiles = 11;

        private string[] soundFontFiles = new string[totalSoundFontFiles] {
           "SoundFonts\\1115_Alaska.sf2",
           "SoundFonts\\1115_Fingered_bass_One.sf2",
           "SoundFonts\\GOOD_DRUMS_6.sf2",
           "SoundFonts\\5_Jazz_Guitar.sf2",
           "SoundFonts\\HS Strings.sf2",
           "SoundFonts\\361_Electric_Piano_Verby.SF2",
           "SoundFonts\\198_Hot_Saxophone_VS!.sf2",
           "SoundFonts\\198_mute_Trumpet.sf2",
           "SoundFonts\\463_Tenor_Sax_1.sf2",
           "SoundFonts\\198_U20_Trombone_VS.sf2",
           "SoundFonts\\JR_voice2.sf2",
        };


        public int PlayerId;
        public bool PlayerInControl;
        private FluidSynthWrapper.Sequencer[] seq = new FluidSynthWrapper.Sequencer[totalSoundFontFiles];
        private FluidSynthWrapper.Synthesizer[] synth = new FluidSynthWrapper.Synthesizer[totalSoundFontFiles];
        private FluidSynthWrapper.AudioDriver[] audioDrv = new FluidSynthWrapper.AudioDriver[totalSoundFontFiles];
        private int[] sfont = new int[totalSoundFontFiles];
        private bool[] mbSFontLoaded = new bool[totalSoundFontFiles];
        private Gesture _pianomiddle2;
        private Gesture _resetstart;
        private Gesture _startPose1;
        private Gesture _startPose3;
        private Gesture _startingPose;
        private string finderdownkey;
        private Gesture _swipeup;
        private Gesture _swipedown;
        private Gesture _swiperight;

        private void initCmbTranspose()
        {
            // http://stackoverflow.com/questions/3063320/combobox-adding-text-and-value-to-an-item-no-binding-source
            Dictionary<string, string> transposeDict = new Dictionary<string, string>();
            transposeDict.Add("0", "Bb");
            transposeDict.Add("1", "B");
            transposeDict.Add("2", "C");
            transposeDict.Add("3", "Db");
            transposeDict.Add("4", "D");
            transposeDict.Add("5", "Eb");
            transposeDict.Add("6", "E");
            transposeDict.Add("7", "F");
            transposeDict.Add("8", "Gb");
            transposeDict.Add("9", "G");
            transposeDict.Add("10", "Ab");
            transposeDict.Add("11", "A");
            cmbTranspose.DataSource = new BindingSource(transposeDict, null);
            cmbTranspose.DisplayMember = "Value";
            cmbTranspose.ValueMember = "Key";
            cmbTranspose.SelectedIndex = 0;
        }

        private void unloadSoundSystem()
        {
            for (int soundNumber = 0; soundNumber < soundFontFiles.Length; ++soundNumber)
            {
                if (mbSFontLoaded[soundNumber] && (synth[soundNumber] != null))
                {
                    synth[soundNumber].SFontUnload((uint)sfont[soundNumber], false);
                    mbSFontLoaded[soundNumber] = false;
                }
                if (synth[soundNumber] != null)
                {
                    synth[soundNumber].Dispose();
                    synth[soundNumber] = null;
                }
                if (seq[soundNumber] != null)
                {
                    seq[soundNumber].Dispose();
                    seq[soundNumber] = null;
                }
                if (audioDrv[soundNumber] != null)
                {
                    audioDrv[soundNumber].Dispose();
                    audioDrv[soundNumber] = null;
                }
            }
        }




        public void DoSomething(String a)
        {
            tbTest.Text = a;
        }

        public void DoSomething2()
        {
            tbTest.Text = "PEace";
        }

        private void init()
        {

            if (_sensor != null)
            {
                // Listen for body data.
                _bodySource = _sensor.BodyFrameSource;
                _bodyReader = _bodySource.OpenReader();
                _bodyReader.FrameArrived += BodyReader_FrameArrived;

                // Listen for HD face data.
                _faceSource = new HighDefinitionFaceFrameSource(_sensor);
                _faceReader = _faceSource.OpenReader();
                _faceReader.FrameArrived += FaceReader_FrameArrived;

                _faceModel = new FaceModel();
                _faceAlignment = new FaceAlignment();

                // Start tracking!        
                _sensor.Open();
            }


            bodyHighlight = new DrawingGroup();


            unloadSoundSystem();
            for (int soundNumber = 0; soundNumber < soundFontFiles.Length; ++soundNumber)
            {
                FluidSynthWrapper.Settings fluidSettings = new FluidSynthWrapper.Settings();
                fluidSettings.AudioDriver = "dsound"; // DirectSound for output
                fluidSettings.SynthAudioChannels = 16; // This sets the number of MIDI channels available for use (defaulted to 16). Could change to 256.
                fluidSettings.SynthSampleRate = 44100; // The audio sample rate sent to the audio driver
                synth[soundNumber] = new FluidSynthWrapper.Synthesizer(fluidSettings);
                audioDrv[soundNumber] = new FluidSynthWrapper.AudioDriver(fluidSettings, synth[soundNumber]); // Create the audio driver witht the settings and connected to the synthesizer
                seq[soundNumber] = new FluidSynthWrapper.Sequencer();
                seq[soundNumber].AddSynthesizer(synth[soundNumber]);
                mbSFontLoaded[soundNumber] = false;
                if (FluidSynthWrapper.Synthesizer.IsSoundFont(soundFontFiles[soundNumber]))
                {
                    mbSFontLoaded[soundNumber] = true;
                    sfont[soundNumber] = synth[soundNumber].SFontLoad(soundFontFiles[soundNumber]);
                    synth[soundNumber].SelectSoundFont(0, sfont[0]);
                    //seq.PlayEventAt(FluidSynthWrapper.Event.NoteOn(0, 60, 127), now + 500, false);
                    //seq.PlayEventAt(FluidSynthWrapper.Event.NoteOff(0, 60), now + 1000, false);
                    //System.Threading.Thread.Sleep(2000);
                    //synth.NoteOn(0, 60, 127);
                    //System.Threading.Thread.Sleep(1000);
                    //synth.NoteOff(0, 60);
                }
            }

        }

        private string generateAscendingSemitones(int totalTimeToGenerateMS)
        {
            int channel = 0;
            int soundNumber = 5;
            uint currentTick = seq[soundNumber].GetCurrentTick();
            uint initialTick = currentTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = 700,
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.0f,
                chanceOfMinimNote = 0.0f,
                chanceOfCrotchetNote = 0.5f,
                chanceOfMultipleItem_Triplet = 0.0f,
                chanceOfMultipleItem_Quaver = 0.1f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.0f,
                chanceOfCrotchetRest = 0.5f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.5f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.6f,
                chanceOfPitchChange2ScaleNotes = 0.02f,
                chanceOfPitchChange3ScaleNotes = 0.01f,
                chanceOfPitchChange4ScaleNotes = 0.01f,
                chanceOfPitchChange5ScaleNotes = 0.01f,
                chanceOfPitchChange6ScaleNotes = 0.01f,
                chanceOfPitchChange7ScaleNotes = 0.01f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 0,
                middlePitchInSemitones = 64,
                maxPitchInSemitones = 127,
                transposeOffsetInSemitones = 0,
                forceMiddlePitch = true,
                runSemitonesAscending = true,
                runScaleAscending = false,
                chance1SimulaneousNotes = 1.0f,
                chance2SimulaneousNotes = 0.0f,
                chance3SimulaneousNotes = 0.0f,
                chance4SimulaneousNotes = 0.0f,
                constantTotalSimulanousNotes = true,
                totalSimultanousNotes = 1,
                velocityMinimum = 127,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        private string generateAscendingScale(int totalTimeToGenerateMS, int transposeOffsetInSemitonesParam)
        {
            int channel = 0;
            int soundNumber = 5;
            uint currentTick = seq[soundNumber].GetCurrentTick();
            uint initialTick = currentTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = 700,
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.0f,
                chanceOfMinimNote = 0.0f,
                chanceOfCrotchetNote = 0.5f,
                chanceOfMultipleItem_Triplet = 0.0f,
                chanceOfMultipleItem_Quaver = 0.1f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.0f,
                chanceOfCrotchetRest = 0.5f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.5f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.6f,
                chanceOfPitchChange2ScaleNotes = 0.02f,
                chanceOfPitchChange3ScaleNotes = 0.01f,
                chanceOfPitchChange4ScaleNotes = 0.01f,
                chanceOfPitchChange5ScaleNotes = 0.01f,
                chanceOfPitchChange6ScaleNotes = 0.01f,
                chanceOfPitchChange7ScaleNotes = 0.01f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 0,
                middlePitchInSemitones = 64,
                maxPitchInSemitones = 127,
                transposeOffsetInSemitones = transposeOffsetInSemitonesParam,
                forceMiddlePitch = false,
                runSemitonesAscending = false,
                runScaleAscending = true,
                chance1SimulaneousNotes = 1.0f,
                chance2SimulaneousNotes = 0.0f,
                chance3SimulaneousNotes = 0.0f,
                chance4SimulaneousNotes = 0.0f,
                constantTotalSimulanousNotes = true,
                totalSimultanousNotes = 1,
                velocityMinimum = 127,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        private void generateBeeps()
        {
            int totalTimeToGenerateMS = 10000;
            int channel = 0;
            int soundNumber = 1;
            uint currentTick = seq[soundNumber].GetCurrentTick();
            uint initialTick = currentTick;
            int noteCount = 0;
            while (currentTick - initialTick < totalTimeToGenerateMS)
            {
                noteCount++;
                seq[soundNumber].PlayEventAt(FluidSynthWrapper.Event.NoteOn(channel, 60, 127), currentTick + 100, true);
                seq[soundNumber].PlayEventAt(FluidSynthWrapper.Event.NoteOff(channel, 60), currentTick + 300, true);
                currentTick += 500;
                if ((noteCount % 5) == 0)
                {
                    currentTick += 500;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generateBeeps();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.multiSourceFrameReader != null)
            {
                this.multiSourceFrameReader.Dispose();
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
            }

            if (this.colorImageBitmap != null)
            {
                this.colorImageBitmap.Dispose();
            }

            this.camera.Dispose();
            unloadSoundSystem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            init();
        }

        // synth
        private string generateSound0(int totalTimeToGenerateMS, uint initialTick, int transposeOffsetST)
        {
            int channel = 0;
            int soundNumber = 0;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.02f,
                chanceOfMinimNote = 0.05f,
                chanceOfCrotchetNote = 0.1f,
                chanceOfMultipleItem_Triplet = 0.0f,
                chanceOfMultipleItem_Quaver = 0.0f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.025f,
                chanceOfCrotchetRest = 0.025f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.5f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.4f,
                chanceOfPitchChange2ScaleNotes = 0.2f,
                chanceOfPitchChange3ScaleNotes = 0.1f,
                chanceOfPitchChange4ScaleNotes = 0.1f,
                chanceOfPitchChange5ScaleNotes = 0.1f,
                chanceOfPitchChange6ScaleNotes = 0.05f,
                chanceOfPitchChange7ScaleNotes = 0.05f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 60,
                maxPitchInSemitones = 100,
                transposeOffsetInSemitones = transposeOffsetST,
                forceMiddlePitch = false,
                forcedPitches = new int[] { },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 0.1f,
                chance2SimulaneousNotes = 0.1f,
                chance3SimulaneousNotes = 0.4f,
                chance4SimulaneousNotes = 0.4f,
                constantTotalSimulanousNotes = false,
                totalSimultanousNotes = 4,
                velocityMinimum = 0,
                velocityMaximum = 30
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // bass
        private string generateSound1(int totalTimeToGenerateMS, uint initialTick, int transposeOffsetST)
        {
            int channel = 0;
            int soundNumber = 1;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.05f,
                chanceOfMinimNote = 0.05f,
                chanceOfCrotchetNote = 0.7f,
                chanceOfMultipleItem_Triplet = 0.05f,
                chanceOfMultipleItem_Quaver = 0.05f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.0f,
                chanceOfCrotchetRest = 0.01f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.5f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.6f,
                chanceOfPitchChange2ScaleNotes = 0.02f,
                chanceOfPitchChange3ScaleNotes = 0.01f,
                chanceOfPitchChange4ScaleNotes = 0.01f,
                chanceOfPitchChange5ScaleNotes = 0.01f,
                chanceOfPitchChange6ScaleNotes = 0.01f,
                chanceOfPitchChange7ScaleNotes = 0.01f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40, // min 40 max 80 for bass
                middlePitchInSemitones = 60,
                maxPitchInSemitones = 80,
                transposeOffsetInSemitones = transposeOffsetST,
                forceMiddlePitch = false,
                forcedPitches = new int[] { },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 1.0f,
                chance2SimulaneousNotes = 0.0f,
                chance3SimulaneousNotes = 0.0f,
                chance4SimulaneousNotes = 0.0f,
                constantTotalSimulanousNotes = true,
                totalSimultanousNotes = 1,
                velocityMinimum = 100,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // kick drum
        private string generateSound2(int totalTimeToGenerateMS, uint initialTick)
        {
            int channel = 0;
            int soundNumber = 2;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.0f,
                chanceOfMinimNote = 0.0f,
                chanceOfCrotchetNote = 0.5f,
                chanceOfMultipleItem_Triplet = 0.0f,
                chanceOfMultipleItem_Quaver = 0.1f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.0f,
                chanceOfCrotchetRest = 0.5f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.5f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.6f,
                chanceOfPitchChange2ScaleNotes = 0.02f,
                chanceOfPitchChange3ScaleNotes = 0.01f,
                chanceOfPitchChange4ScaleNotes = 0.01f,
                chanceOfPitchChange5ScaleNotes = 0.01f,
                chanceOfPitchChange6ScaleNotes = 0.01f,
                chanceOfPitchChange7ScaleNotes = 0.01f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 55, // 55=kick 61=closedhihat 66=openhihat 41=triangle 57&59=tom
                maxPitchInSemitones = 80,
                transposeOffsetInSemitones = 0,
                forceMiddlePitch = true,
                forcedPitches = new int[] { },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 1.0f,
                chance2SimulaneousNotes = 0.0f,
                chance3SimulaneousNotes = 0.0f,
                chance4SimulaneousNotes = 0.0f,
                constantTotalSimulanousNotes = true,
                totalSimultanousNotes = 1,
                velocityMinimum = 100,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // closed hihat and open hihat
        private string generateSound2B(int totalTimeToGenerateMS, uint initialTick)
        {
            int channel = 0;
            int soundNumber = 2;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.0f,
                chanceOfMinimNote = 0.0f,
                chanceOfCrotchetNote = 0.0f,
                chanceOfMultipleItem_Triplet = 0.0f,
                chanceOfMultipleItem_Quaver = 0.9f,
                chanceOfMultipleItem_Semiquaver = 0.1f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.0f,
                chanceOfCrotchetRest = 0.0f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.7f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.7f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.6f,
                chanceOfPitchChange2ScaleNotes = 0.02f,
                chanceOfPitchChange3ScaleNotes = 0.01f,
                chanceOfPitchChange4ScaleNotes = 0.01f,
                chanceOfPitchChange5ScaleNotes = 0.01f,
                chanceOfPitchChange6ScaleNotes = 0.01f,
                chanceOfPitchChange7ScaleNotes = 0.01f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 61, // 55=kick 61=closedhihat 66=openhihat 41=triangle 57&59=tom
                maxPitchInSemitones = 80,
                transposeOffsetInSemitones = 0,
                forceMiddlePitch = false,
                forcedPitches = new int[] { 61, 66 },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 1.0f,
                chance2SimulaneousNotes = 0.0f,
                chance3SimulaneousNotes = 0.0f,
                chance4SimulaneousNotes = 0.0f,
                constantTotalSimulanousNotes = true,
                totalSimultanousNotes = 1,
                velocityMinimum = 100,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // triangle, tom, snare
        private string generateSound2C(int totalTimeToGenerateMS, uint initialTick)
        {
            int channel = 0;
            int soundNumber = 2;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.0f,
                chanceOfMinimNote = 0.0f,
                chanceOfCrotchetNote = 0.0f,
                chanceOfMultipleItem_Triplet = 0.0f,
                chanceOfMultipleItem_Quaver = 0.5f,
                chanceOfMultipleItem_Semiquaver = 0.5f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.0f,
                chanceOfCrotchetRest = 0.0f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.3f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.3f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.6f,
                chanceOfPitchChange2ScaleNotes = 0.02f,
                chanceOfPitchChange3ScaleNotes = 0.01f,
                chanceOfPitchChange4ScaleNotes = 0.01f,
                chanceOfPitchChange5ScaleNotes = 0.01f,
                chanceOfPitchChange6ScaleNotes = 0.01f,
                chanceOfPitchChange7ScaleNotes = 0.01f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 41, // 55=kick 61=closedhihat 66=openhihat 41=triangle 60=tom 63=snare
                maxPitchInSemitones = 80,
                transposeOffsetInSemitones = 0,
                forceMiddlePitch = false,
                forcedPitches = new int[] { 41, 60, 63 },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 1.0f,
                chance2SimulaneousNotes = 0.0f,
                chance3SimulaneousNotes = 0.0f,
                chance4SimulaneousNotes = 0.0f,
                constantTotalSimulanousNotes = true,
                totalSimultanousNotes = 1,
                velocityMinimum = 100,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // jazz guitar
        private string generateSound3(int totalTimeToGenerateMS, uint initialTick, int transposeOffsetST)
        {
            int channel = 0;
            int soundNumber = 3;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.02f,
                chanceOfMinimNote = 0.05f,
                chanceOfCrotchetNote = 0.1f,
                chanceOfMultipleItem_Triplet = 0.1f,
                chanceOfMultipleItem_Quaver = 0.1f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.025f,
                chanceOfCrotchetRest = 0.025f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.5f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.4f,
                chanceOfPitchChange2ScaleNotes = 0.2f,
                chanceOfPitchChange3ScaleNotes = 0.1f,
                chanceOfPitchChange4ScaleNotes = 0.1f,
                chanceOfPitchChange5ScaleNotes = 0.1f,
                chanceOfPitchChange6ScaleNotes = 0.05f,
                chanceOfPitchChange7ScaleNotes = 0.05f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 60,
                maxPitchInSemitones = 100,
                transposeOffsetInSemitones = transposeOffsetST,
                forceMiddlePitch = false,
                forcedPitches = new int[] { },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 0.1f,
                chance2SimulaneousNotes = 0.1f,
                chance3SimulaneousNotes = 0.4f,
                chance4SimulaneousNotes = 0.4f,
                constantTotalSimulanousNotes = false,
                totalSimultanousNotes = 4,
                velocityMinimum = 100,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // strings
        private string generateSound4(int totalTimeToGenerateMS, uint initialTick, int transposeOffsetST)
        {
            int channel = 0;
            int soundNumber = 4;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.7f,
                chanceOfMinimNote = 0.3f,
                chanceOfCrotchetNote = 0.0f,
                chanceOfMultipleItem_Triplet = 0.0f,
                chanceOfMultipleItem_Quaver = 0.0f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0f,
                chanceOfMinimRest = 0.025f,
                chanceOfCrotchetRest = 0.025f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.6f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.5f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.4f,
                chanceOfPitchChange2ScaleNotes = 0.2f,
                chanceOfPitchChange3ScaleNotes = 0.1f,
                chanceOfPitchChange4ScaleNotes = 0.1f,
                chanceOfPitchChange5ScaleNotes = 0.1f,
                chanceOfPitchChange6ScaleNotes = 0.05f,
                chanceOfPitchChange7ScaleNotes = 0.05f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 60,
                maxPitchInSemitones = 100,
                transposeOffsetInSemitones = transposeOffsetST,
                forceMiddlePitch = false,
                forcedPitches = new int[] { },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 0.7f,
                chance2SimulaneousNotes = 0.1f,
                chance3SimulaneousNotes = 0.1f,
                chance4SimulaneousNotes = 0.1f,
                constantTotalSimulanousNotes = false,
                totalSimultanousNotes = 4,
                velocityMinimum = 30,
                velocityMaximum = 60
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // electric piano
        private string generateSound5(int totalTimeToGenerateMS, uint initialTick, int transposeOffsetST)
        {
            int channel = 0;
            int soundNumber = 5;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.05f,
                chanceOfMinimNote = 0.05f,
                chanceOfCrotchetNote = 0.1f,
                chanceOfMultipleItem_Triplet = 0.2f,
                chanceOfMultipleItem_Quaver = 0.3f,
                chanceOfMultipleItem_Semiquaver = 0.8f,
                chanceOfSemibreveRest = 0.05f,
                chanceOfMinimRest = 0.05f,
                chanceOfCrotchetRest = 0.05f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.7f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.8f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.5f,
                chanceOfPitchChange2ScaleNotes = 0.2f,
                chanceOfPitchChange3ScaleNotes = 0.05f,
                chanceOfPitchChange4ScaleNotes = 0.05f,
                chanceOfPitchChange5ScaleNotes = 0.1f,
                chanceOfPitchChange6ScaleNotes = 0.05f,
                chanceOfPitchChange7ScaleNotes = 0.05f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 60,
                maxPitchInSemitones = 100,
                transposeOffsetInSemitones = transposeOffsetST,
                forceMiddlePitch = false,
                forcedPitches = new int[] { },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 0.9f,
                chance2SimulaneousNotes = 0.05f,
                chance3SimulaneousNotes = 0.025f,
                chance4SimulaneousNotes = 0.025f,
                constantTotalSimulanousNotes = false,
                totalSimultanousNotes = 4,
                velocityMinimum = 100,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            cNoteGenerator.close();
            return noteHistory;
        }

        // horns (soundNumber 6,7,8,9) and vocals (soundNumber 10)
        private string generateSoundX(int totalTimeToGenerateMS, uint initialTick, int soundNumber,
            ref uint finalTick, int transposeOffsetST)
        {
            int channel = 0;
            uint currentTick = initialTick;
            clsNoteGenerator cNoteGenerator = new clsNoteGenerator(currentTick, seq[soundNumber], channel)
            {
                speed_beatsPerMinute = Convert.ToInt32(tbSpeed.Text),
                macro_numCrotchets = 8 * 4,
                chanceOfSemibreveNote = 0.025f,
                chanceOfMinimNote = 0.05f,
                chanceOfCrotchetNote = 0.3f,
                chanceOfMultipleItem_Triplet = 0.3f,
                chanceOfMultipleItem_Quaver = 0.2f,
                chanceOfMultipleItem_Semiquaver = 0.0f,
                chanceOfSemibreveRest = 0.0255f,
                chanceOfMinimRest = 0.05f,
                chanceOfCrotchetRest = 0.05f,
                quaverSwingPercent = 66,
                chanceOfTie = 0.1f,
                chanceOfTieInMultipleItem_Triplet = 0.1f,
                chanceOfNoteInMultipleItem_Triplet = 0.7f,
                chanceOfTieInMultipleItem_Quaver = 0.1f,
                chanceOfNoteInMultipleItem_Quaver = 0.8f,
                chanceOfTieInMultipleItem_Semiquaver = 0.1f,
                chanceOfNoteInMultipleItem_Semiquaver = 0.8f,
                chanceOfPitchChange0ScaleNotes = 0.0f,
                chanceOfPitchChange1ScaleNotes = 0.6f,
                chanceOfPitchChange2ScaleNotes = 0.15f,
                chanceOfPitchChange3ScaleNotes = 0.05f,
                chanceOfPitchChange4ScaleNotes = 0.05f,
                chanceOfPitchChange5ScaleNotes = 0.05f,
                chanceOfPitchChange6ScaleNotes = 0.05f,
                chanceOfPitchChange7ScaleNotes = 0.05f,
                scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 },
                minPitchInSemitones = 40,
                middlePitchInSemitones = 60,
                maxPitchInSemitones = 90,
                transposeOffsetInSemitones = transposeOffsetST,
                forceMiddlePitch = false,
                forcedPitches = new int[] { },
                runSemitonesAscending = false,
                runScaleAscending = false,
                chance1SimulaneousNotes = 1.0f,
                chance2SimulaneousNotes = 0.0f,
                chance3SimulaneousNotes = 0.0f,
                chance4SimulaneousNotes = 0.0f,
                constantTotalSimulanousNotes = true,
                totalSimultanousNotes = 1,
                velocityMinimum = 100,
                velocityMaximum = 127
            };
            while (cNoteGenerator.getCurrentComposingTick() - initialTick < totalTimeToGenerateMS)
            {
                cNoteGenerator.runNoteOnOffChanges();
            }
            string noteHistory = cNoteGenerator.getMusicHistory();
            finalTick = cNoteGenerator.getCurrentComposingTick();
            cNoteGenerator.close();
            return noteHistory;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string fingerDown = Switzlyphone.Properties.Settings.Default.fingerdown;


            if (Switzlyphone.Properties.Settings.Default.startpose == 1 && ApplicationSettings.Load(fingerDown) == 0)
            {


                ApplicationSettings.Save(1, fingerDown);
                ApplicationSettings.Save(0, "startpose");

                int totalTimeToGenerateMS = Convert.ToInt32(tbTotalTimeMS.Text); // eg 60000
                int transposeOffsetInSemitones = Convert.ToInt32(((KeyValuePair<string, string>)cmbTranspose.SelectedItem).Key.ToString());
                uint[] initialTick = new uint[totalSoundFontFiles];
                for (int soundNumber = 0; soundNumber < totalSoundFontFiles; ++soundNumber)
                {
                    initialTick[soundNumber] = seq[soundNumber].GetCurrentTick();
                }
                tbMusicHistory.Text = "";
                if (cbSynth.Checked)
                {
                    tbMusicHistory.Text += "SOUND0(Synth)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound0(totalTimeToGenerateMS, initialTick[0], transposeOffsetInSemitones);
                }
                if (cbBass.Checked)
                {
                    tbMusicHistory.Text += "SOUND1(Bass)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound1(totalTimeToGenerateMS, initialTick[1], transposeOffsetInSemitones);
                }
                if (cbKickDrum.Checked)
                {
                    tbMusicHistory.Text += "SOUND2(Kick)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound2(totalTimeToGenerateMS, initialTick[2]);
                }
                if (cbClosedAndOpenHihat.Checked)
                {
                    tbMusicHistory.Text += "SOUND2B(Closed&OpenHihat)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound2B(totalTimeToGenerateMS, initialTick[2]);
                }
                if (cbTriangleAndTomAndSnare.Checked)
                {
                    tbMusicHistory.Text += "SOUND2C(Triangle&Tom&Snare)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound2C(totalTimeToGenerateMS, initialTick[2]);
                }
                if (cbGuitar.Checked)
                {
                    tbMusicHistory.Text += "SOUND3(JazzGuitar)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound3(totalTimeToGenerateMS, initialTick[3], transposeOffsetInSemitones);
                }
                if (cbStrings.Checked)
                {
                    tbMusicHistory.Text += "SOUND4(Strings)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound4(totalTimeToGenerateMS, initialTick[4], transposeOffsetInSemitones);
                }
                if (cbElectricPiano.Checked)
                {
                    tbMusicHistory.Text += "SOUND5(ElectricPiano)" + Environment.NewLine;
                    tbMusicHistory.Text += generateSound5(totalTimeToGenerateMS, initialTick[5], transposeOffsetInSemitones);
                }
                if (cbEnableMelody.Checked)
                {
                    uint groupTickStart = initialTick[6];
                    uint finalTick = 0;
                    int melodyInstrumentCount = 0;
                    if (cbHorn1_Sax.Checked) melodyInstrumentCount++;
                    if (cbHorn2_Trumpet.Checked) melodyInstrumentCount++;
                    if (cbHorn3_TenorSax.Checked) melodyInstrumentCount++;
                    if (cbHorn4_Trombone.Checked) melodyInstrumentCount++;
                    if (cbVocals.Checked) melodyInstrumentCount++;
                    if (melodyInstrumentCount > 0)
                    {
                        for (int soundNumber = 6; soundNumber <= 10; ++soundNumber)
                        {
                            if ((soundNumber == 6 && cbHorn1_Sax.Checked) ||
                                (soundNumber == 7 && cbHorn2_Trumpet.Checked) ||
                                (soundNumber == 8 && cbHorn3_TenorSax.Checked) ||
                                (soundNumber == 9 && cbHorn4_Trombone.Checked) ||
                                (soundNumber == 10 && cbVocals.Checked))
                            {
                                tbMusicHistory.Text += "SOUND" + soundNumber.ToString() + "(Horns)" + Environment.NewLine;
                                tbMusicHistory.Text += generateSoundX((int)Math.Round(totalTimeToGenerateMS / (float)melodyInstrumentCount), groupTickStart, soundNumber, ref finalTick, transposeOffsetInSemitones);
                                groupTickStart = finalTick;
                            }
                        }
                    }
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int totalTimeToGenerateMS = 60000;
            generateAscendingSemitones(totalTimeToGenerateMS);
        }

        private void cbEnableHorns_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbEnableMelody.Checked)
            {
                cbHorn1_Sax.Enabled = false;
                cbHorn2_Trumpet.Enabled = false;
                cbHorn3_TenorSax.Enabled = false;
                cbHorn4_Trombone.Enabled = false;
                cbVocals.Enabled = false;
            }
            else
            {
                cbHorn1_Sax.Enabled = true;
                cbHorn2_Trumpet.Enabled = true;
                cbHorn3_TenorSax.Enabled = true;
                cbHorn4_Trombone.Enabled = true;
                cbVocals.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int totalTimeToGenerateMS = 60000;
            int transposeOffsetInSemitones = Convert.ToInt32(((KeyValuePair<string, string>)cmbTranspose.SelectedItem).Key.ToString());
            generateAscendingScale(totalTimeToGenerateMS, transposeOffsetInSemitones);
        }

        private void FrmMusicMaker_Load(object sender, EventArgs e)
        {

            if (KinectSensor.GetDefault() != null)
            {
                this.kinectSensor = KinectSensor.GetDefault();

                if (this.kinectSensor != null)
                {
                    this.kinectSensor.Open();

                    FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;
                    this.colorImagePixelData = new byte[colorFrameDescription.Width * colorFrameDescription.Height * 4];
                    FrameDescription irleFrameDescription = this.kinectSensor.LongExposureInfraredFrameSource.FrameDescription;
                    this.irLEImagePixelData = new ushort[irleFrameDescription.LengthInPixels];
                    FrameDescription irFrameDescription = this.kinectSensor.InfraredFrameSource.FrameDescription;
                    this.irImagePixelData = new ushort[irFrameDescription.Width * irFrameDescription.Height];
                    this.irImagePixelDataOld = new ushort[irFrameDescription.Width * irFrameDescription.Height];

                    this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;
                    this.bodies = new Body[this.bodyCount];
                    this.multiSourceFrameReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Infrared | FrameSourceTypes.Depth | FrameSourceTypes.Color | FrameSourceTypes.Body | FrameSourceTypes.LongExposureInfrared);

                }
            }


        }


        public static System.Drawing.Bitmap BitmapSourceToBitmap2(BitmapSource srs)
        {
            int width = srs.PixelWidth;
            int height = srs.PixelHeight;
            int stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                srs.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
                using (var btm = new System.Drawing.Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr))
                {
                    // Clone the bitmap so that we can dispose it and
                    // release the unmanaged memory at ptr
                    return new System.Drawing.Bitmap(btm);
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public enum Mode
        {
            Color,
            Depth,
            Infrared,
            LongIR,
            ColorDepth
        }


        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            if (!cbSkeleton.Checked)
            {
                cbBodies.Enabled = false;
                cbBody.Enabled = false;
                cbMode.Enabled = false;
                return;
            }
            if (cbSkeleton.Checked)
            {
                cbBodies.Enabled = true;
                cbBody.Enabled = true;
                cbMode.Enabled = true;

                var reference = e.FrameReference.AcquireFrame();


                // Color
                using (var frame = reference.ColorFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        if (_mode == Mode.Color)
                        {


                            camera.Image = ImageWpfToGDI(ToBitmap2(frame));
                        }
                    }
                }

                // colorDepth
                using (var frame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        if (_mode == Mode.ColorDepth)
                        {


                            useDepthFrame(reference.DepthFrameReference);
                        }
                    }
                }
                // Depth
                using (var frame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        if (_mode == Mode.Depth)
                        {


                            camera.Image = ImageWpfToGDI(frame.ToBitmap());
                        }
                    }
                }

                // Infrared
                using (var frame = reference.InfraredFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        if (_mode == Mode.Infrared)
                        {

                            camera.Image = ImageWpfToGDI(frame.ToBitmap());
                        }
                    }
                }
                // LongExposureInfrared
                using (var frame = reference.LongExposureInfraredFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        if (_mode == Mode.LongIR)
                        {
                            LongExposureInfraredFrameReference leirFrameReference = reference.LongExposureInfraredFrameReference;

                            useLIRFrame(leirFrameReference);

                        }
                    }


                }


                if (cbBody.Checked)
                {

                    using (var frame2 = reference.BodyFrameReference.AcquireFrame())
                    {
                        if (frame2 != null)
                        {
                            _bodies = new Body[frame2.BodyFrameSource.BodyCount];

                            frame2.GetAndRefreshBodyData(_bodies);
                            foreach (var body in _bodies)
                            {
                                if (body != null)
                                {
                                    if (body.IsTracked)
                                    {

                                        // Draw skeleton.
                                        if (_displayBody)
                                        {
                                            //    canvas.DrawSkeleton(body);
                                            if (body.Joints[JointType.HandRight].TrackingState == TrackingState.Tracked && body.Joints[JointType.HandLeft].TrackingState == TrackingState.Tracked)

                                            // if (body.Joints[JointType.HandRight].TrackingState == TrackingState.Tracked )
                                            {
                                                //    canvas.DrawSkeleton(body);
                                                if (body.Joints[JointType.Head].TrackingState == TrackingState.Tracked)
                                                {
                                                    System.Drawing.Pen pen;

                                                    var gobject = Graphics.FromImage(camera.Image);

                                                    pen = new System.Drawing.Pen(System.Drawing.Color.Green, 5);
                                                    var HandLeft = body.Joints[JointType.HandLeft].Position;
                                                    var HandRight = body.Joints[JointType.HandRight].Position;
                                                    DepthSpacePoint point2 = _sensor.CoordinateMapper.MapCameraPointToDepthSpace(HandRight);
                                                    DepthSpacePoint point = _sensor.CoordinateMapper.MapCameraPointToDepthSpace(HandLeft);
                                                    float x = 0f;
                                                    float y = 0f;
                                                    if (!float.IsInfinity(point.X) && !float.IsInfinity(point.Y))
                                                    {
                                                        x = point.X;
                                                        y = point.Y;
                                                    }
                                                    System.Windows.Point mappedPoint = new System.Windows.Point();
                                                    mappedPoint.X = x;
                                                    mappedPoint.Y = y;
                                                    float x2 = 0f;
                                                    float y2 = 0f;
                                                    if (!float.IsInfinity(point2.X) && !float.IsInfinity(point2.Y))
                                                    {
                                                        x2 = point2.X;
                                                        y2 = point2.Y;
                                                    }
                                                    System.Windows.Point mappedPoint2 = new System.Windows.Point();
                                                    mappedPoint2.X = x2;
                                                    mappedPoint2.Y = y2;

                                                    float confi3 = 30f + (int)body.HandLeftConfidence * 20f;


                                                    gobject.DrawEllipse(pen, x - 10, y, confi3, confi3);
                                                    pen = new System.Drawing.Pen(System.Drawing.Color.Red, 5);
                                                    float conf2i = 30f + (int)body.HandRightConfidence * 20f;

                                                    gobject.DrawEllipse(pen, x2 - 10, y2, conf2i, conf2i);
                                                    //   gobject.DrawLine(pen, head.X, head.Y, neck.X, neck.Y);
                                                }

                                                // Updates the onscreen labels with current data
                                                handLx.Text = body.Joints[JointType.HandLeft].Position.X.ToString();
                                                handLy.Text = body.Joints[JointType.HandLeft].Position.Y.ToString();
                                                handLz.Text = body.Joints[JointType.HandLeft].Position.Z.ToString();
                                                handRx.Text = body.Joints[JointType.HandRight].Position.X.ToString();
                                                handRy.Text = body.Joints[JointType.HandRight].Position.Y.ToString();
                                                handRz.Text = body.Joints[JointType.HandRight].Position.Z.ToString();

                                                //          currentPitch = pitchReference + body.Joints[JointType.HandRight].Position.Y;


                                                //pitch increases as right hand moves up, volume increases as left hand moves up
                                                if (body.Joints[JointType.HandLeft].Position.Y >= -0.5)
                                                {
                                                    //  currentPitch = body.Joints[JointType.HandRight].Position.Y / (1 / 6) * -1 + pitchReference;
                                                    //     }
                                                    //          if (body.Joints[JointType.HandRight].Position.Y >= -0.35 && body.Joints[JointType.HandRight].Position.Y < 0) currentPitch = body.Joints[JointType.HandRight].Position.Y * -50 + pitchReference;

                                                    //         if (body.Joints[JointType.HandRight].Position.Y >= 0) currentPitch = body.Joints[JointType.HandRight].Position.Y + 0.2 * 50 + pitchReference;

                                                    //  osc.Frequency = freqReference * Math.Pow(freqRatio, currentPitch - pitchReference);
                                                    //        osc.Frequency = currentPitch * 5 * body.Joints[JointType.HandRight].Position.X * 4;
                                                    //      if (body.Joints[JointType.HandLeft].Position.Y >= 0.53 || body.Joints[JointType.HandLeft].Position.Y < 0 || body.Joints[JointType.HandLeft].Position.Y >= -0.5) osc.Amplitude = 0.63 / .06 * 2716;
                                                    //      if (body.Joints[JointType.HandLeft].Position.Y <= 0.52 && body.Joints[JointType.HandLeft].Position.Y >= 0) osc.Amplitude = body.Joints[JointType.HandLeft].Position.Y / .06 * 2716;
                                                    //       osc.Frequency = currentPitch * 5 * body.Joints[JointType.HandRight].Position.X * 4;
                                                    //    if ((body.HandLeftState == HandState.Closed && body.Joints[JointType.HandLeft].Position.Y >= 0.53) || (body.HandLeftState == HandState.Closed && body.Joints[JointType.HandLeft].Position.Y < 0)) osc.Amplitude = 0.53 / .06 * 2716;
                                                    //     if (body.HandLeftState == HandState.Closed && body.Joints[JointType.HandLeft].Position.Y <= 0.52 && body.Joints[JointType.HandLeft].Position.Y >= 0) osc.Amplitude = body.Joints[JointType.HandLeft].Position.Y / .06 * 2716;

                                                    //              if (waveOut != null)
                                                    //         {
                                                    //      waveOut.DesiredLatency = 0;
                                                    //       waveOut.Play();
                                                    //    }

                                                    //frequency output on screen in label
                                                    //                                   freqLabel.Text = osc.Frequency.ToString();


                                                    // }
                                                    //  else { waveOut.Stop(); }


                                                }



                                            }



                                            if (body.HandLeftState == HandState.Open) { }

                                            if (body.HandLeftState == HandState.Closed)
                                            {
                                                float confi4 = 30f + (int)body.HandLeftConfidence * 20f;

                                                if (confi4 >= -0.5)
                                                {
                                                    int ms = Convert.ToInt32(tbTotalTimeMS.Text);
                                                    if (body.Joints[JointType.HandLeft].Position.Y >= -0.5) ms = ms + Convert.ToInt32((body.Joints[JointType.HandLeft].Position.Y * 30));

                                                    if (body.Joints[JointType.HandLeft].Position.Y < 0) ms = ms + Convert.ToInt32((body.Joints[JointType.HandLeft].Position.Y * 30));


                                                    tbTotalTimeMS.Text = ms.ToString();

                                                }
                                            }

                                            if (body.HandLeftState == HandState.Lasso)
                                            {
                                                //  button3.PerformClick();
                                                if (body.HandRightState == HandState.Lasso)
                                                {
                                                    button3.PerformClick();

                                                }
                                            }


                                            if (body.HandLeftState == HandState.NotTracked) { }
                                            if (body.HandLeftState == HandState.Unknown) { }

                                            //Lean   if (body.Lean )   Gets the amount a body is leaning, which is a number between - 1(leaning left or back) and 1(leaning right or front).  }

                                            // if (body.HandRightState == HandState.Closed){
                                            float confi = 30f + (int)body.HandLeftConfidence * 20f;

                                            if (confi >= -0.5)
                                            {
                                                if (body.Joints[JointType.HandRight].Position.X >= -0.8)
                                                {
                                                    int speed = Convert.ToInt32(tbSpeed.Text);
                                                    speed = Convert.ToInt32((body.Joints[JointType.HandRight].Position.X * 540));

                                                    if (speed <= 50) speed = 50;
                                                    tbSpeed.Text = speed.ToString();
                                                    freqLabel.Text = confi.ToString();
                                                }
                                                if (body.Joints[JointType.HandRight].Position.Y >= -0.5)
                                                {

                                                    int tone = cmbTranspose.SelectedIndex;
                                                    tone = Convert.ToInt32((body.Joints[JointType.HandRight].Position.Y * 40));
                                                    if (tone >= 12) tone = 11;
                                                    if (tone <= 00) tone = 0;
                                                    cmbTranspose.SelectedIndex = tone;
                                                    freqLabel.Text = tone.ToString();
                                                    //   }
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }
                    }
                }


                if (cbBodies.Checked)
                {
                    // Body
                    using (var frame4 = reference.BodyFrameReference.AcquireFrame())
                    {
                        if (frame4 != null)
                        {
                            // get a reader

                            BodyFrameReader reader = _sensor.BodyFrameSource.OpenReader();
                            // make the event into an observable
                            var obsFrameEvents = Observable.FromEventPattern<BodyFrameArrivedEventArgs>(
                              handler => reader.FrameArrived += handler,
                              handler => reader.FrameArrived -= handler);

                            // acquire the frames that we can
                            var obsBodies = obsFrameEvents
                              .Select(frame44 => frame44.EventArgs.FrameReference.AcquireFrame())
                              .Where(frame44 => frame44 != null)
                              .Select(frame44 =>
                              {
                                  Body[] bodies2 = new Body[frame44.BodyCount];
                                  frame44.GetAndRefreshBodyData(bodies2);
                                  frame44.Dispose();
                                  return (bodies2);
                              }
                              );



                            var obsTrackedBodies = obsBodies
                                .Select(
                                  bodies =>
                                  {
                                      return (bodies
                    .Where(b => b.IsTracked)
                    .OrderBy(b => b.TrackingId) // assumption that this increases as bodies arrive (TBD!)
                    .Select((b, i) => new { BodyNo = i, Body = b }));
                                  }
                              );

                            var obsBodyCount = obsTrackedBodies
          .Select(bodies => bodies.Count());

                            obsBodyCount
                              .Subscribe(
                                c =>
                                {
                                    lbBodies.Text = c.ToString();
                                }
                              );

                            var obsFirstBody = obsTrackedBodies
                              .SelectMany(b => b)
                              .Where(b => b.BodyNo == 0);

                            // e.g. now extract the left hand position
                            var obsLeftHandXPosition = obsFirstBody
                              .Where(b => b.Body.Joints[JointType.HandLeft].TrackingState == TrackingState.Tracked)
                              .Select(b => b.Body.Joints[JointType.HandLeft].Position.X);

                            obsLeftHandXPosition
                              .Subscribe(
                                x =>
                                {
                                    lb1x.Text = x.ToString();
                                }
                              );








                        }

                    }
                }
            }
        }
        System.Drawing.Image ImageWpfToGDI(System.Windows.Media.ImageSource image)
        {
            MemoryStream ms = new MemoryStream();
            var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image as System.Windows.Media.Imaging.BitmapSource));
            encoder.Save(ms);
            ms.Flush();
            return System.Drawing.Image.FromStream(ms);
        }
        static string CameraSpacePointToString(CameraSpacePoint point)
        {
            return (string.Format("[{0:G3},{1:G3},{2:G3}]", point.X, point.Y, point.Z));
        }
        class SineWaveOscillator : WaveProvider16
        //https://msdn.microsoft.com/en-us/magazine/ee309883.aspx
        {
            double phaseAngle; //ranges between 0 and 2*PI

            public SineWaveOscillator(int sampleRate) :
                base(sampleRate, 1)
            {
            }

            public double Frequency { set; get; }
            public double Amplitude { set; get; }

            //called 10 times a second(default)
            //fills buffer with waveform data
            public override int Read(short[] buffer, int offset, int sampleCount)
            {

                //for each sample(taken 10 times a second)
                for (int index = 0; index < sampleCount; index++)
                {
                    //pass phaseAngle to Math.Sin and add to buffer
                    buffer[offset + index] = (short)(Amplitude * Math.Sin(phaseAngle));

                    //increase by phase angle increment
                    phaseAngle += 2 * Math.PI * Frequency / WaveFormat.SampleRate;

                    //ensures angle doesn't exceed 2*PI
                    if (phaseAngle > 2 * Math.PI)
                    {
                        phaseAngle -= 2 * Math.PI;
                    }
                }
                return sampleCount;
            }
        }
        static double CameraSpaceDistance(CameraSpacePoint p1, CameraSpacePoint p2)
        {
            return (
              Math.Sqrt(
                Math.Pow(p1.X - p2.X, 2) +
                Math.Pow(p1.Y - p2.Y, 2) +
                Math.Pow(p1.Z - p2.Z, 2)));
        }
        private void handLx_Click(object sender, EventArgs e)
        {

        }
        public enum JointRelationship
        {
            None,
            Above,
            Below,
            LeftOf,
            RightOf,
            AboveAndRight,
            BelowAndRight,
            AboveAndLeft,
            BelowAndLeft
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMode.SelectedItem.ToString() == "Depth") _mode = Mode.Depth;
            if (cbMode.SelectedItem.ToString() == "Color") _mode = Mode.Color;
            if (cbMode.SelectedItem.ToString() == "Infrared") _mode = Mode.Infrared;
            if (cbMode.SelectedItem.ToString() == "LongIR") _mode = Mode.LongIR;
            if (cbMode.SelectedItem.ToString() == "ColorDepth") _mode = Mode.ColorDepth;
        }

        private void tbTotalTimeMS_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tbMusicHistory_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbBodies_Click(object sender, EventArgs e)
        {

        }

        private void cbGuitar_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void VolumeToggleMute()
        {
            tbTest.Text = "mute";
        }

        public void VolumeUp()
        {
            tbTest.Text = "up";
        }

        public void VolumeDown()
        {
            tbTest.Text = "down";
        }
        public void SwipeLeft()
        {
            tbTest.Text = "sleft";
        }
        public void SwipeRight()
        {
            tbTest.Text = "sright";
        }
        public void SwipeUp()
        {
            tbTest.Text = "sup";
        }
        public void SwipeDown()
        {
            tbTest.Text = "sdown";
        }
        /// <summary>
        /// Helper method to determin if invoke required, if so will rerun method on correct thread.
        /// if not do nothing.
        /// </summary>
        /// <param name="c">Control that might require invoking</param>
        /// <param name="a">action to preform on control thread if so.</param>
        /// <returns>true if invoke required</returns>
        public bool ControlInvokeRequired(System.Windows.Forms.Control c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;

            return true;
        }
        public void UpdateTextBox1(String text)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(tbTest, () => UpdateTextBox1(text))) return;
            tbTest.Text = text;
        }
        //Or any control
        public void UpdateControl(Control ctrl)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(ctrl, () => UpdateControl(ctrl))) return;

        }
        private async void CbHands_CheckedChangedAsync(object sender, EventArgs e)
        {
            var hold = new HandPose("Hold", new FingerPose(new[] { Finger.Thumb, Finger.Index }, FingerFlexion.Open, PoseDirection.Forward),
                                             new FingertipDistanceRelation(Finger.Index, RelativeDistance.NotTouching, Finger.Thumb),
                                             new FingertipPlacementRelation(Finger.Index, RelativePlacement.Above, Finger.Thumb));
            var rotate = new HandPose("Rotate", new FingerPose(new[] { Finger.Thumb, Finger.Index }, FingerFlexion.Open, PoseDirection.Forward),
                                                new FingertipDistanceRelation(Finger.Index, RelativeDistance.NotTouching, Finger.Thumb),
                                                new FingertipPlacementRelation(Finger.Index, RelativePlacement.Right, Finger.Thumb));
            var poses = new List<HandPose>();

            var allFingersContext = new AllFingersContext();

            // Hand starts upright, forward and with fingers spread...
            var startPose = new HandPose(
              "start", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
             new FingerPose(
               allFingersContext, FingerFlexion.Open),
             new FingertipDistanceRelation(Finger.Middle, RelativeDistance.Touching, new[] { Finger.Index, Finger.Ring }),
            new FingertipDistanceRelation(
                    new[] { Finger.Index, Finger.Middle }, RelativeDistance.Touching));
            poses.Add(startPose);

            foreach (Finger finger in
              new[] { Finger.Index, Finger.Middle, Finger.Ring, Finger.Pinky })
            {
                poses.Add(
                  new HandPose(
                  $"pose{finger}", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
                  new FingertipDistanceRelation(
                    Finger.Thumb, RelativeDistance.NotTouching, finger)));
            }


            // Hand starts forward and with fingers spread...
            var start2Pose = new HandPose(
  "start2pose", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
new FingerPose(

   allFingersContext, FingerFlexion.Open, PoseDirection.Forward));
            new FingertipDistanceRelation(
              allFingersContext, RelativeDistance.NotTouching);

            poses.Add(start2Pose);

            foreach (Finger finger in
               new[] { Finger.Index, Finger.Middle, Finger.Ring, Finger.Pinky })
            {
                poses.Add(
                  new HandPose(
                  $"pose{finger}",
                  new FingertipDistanceRelation(
                    Finger.Thumb, RelativeDistance.NotTouching, finger)));
            }






            var startingPose = new HandPose("startingpose");
            startingPose.PoseConstraints.Add(new AnyFingerContext(new[] { Finger.Middle, Finger.Index, Finger.Pinky, Finger.Thumb, Finger.Ring }), PoseDirection.Down);


            // var startingPose = GenerateOpenFingersPose("startingpose", new[] { Finger.Index, Finger.Middle, Finger.Ring, Finger.Pinky });







            var start22Pose = new HandPose(
         "start22pose", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
           new FingerPose(
               allFingersContext, FingerFlexion.Open),
             new FingertipDistanceRelation(
               allFingersContext, RelativeDistance.NotTouching));

            poses.Add(start22Pose);

            foreach (Finger finger in
              new[] { Finger.Index, Finger.Middle, Finger.Ring, Finger.Pinky })
            {
                poses.Add(
                  new HandPose(
                  $"pose{finger}", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
                  new FingertipDistanceRelation(
                    Finger.Thumb, RelativeDistance.NotTouching, finger)));
            }

            var startingPose1 = startingPose;
            startingPose1.Name = "startingIndex";
            var startPose1 = new HandPose(
    "startIndex", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
    new FingerPose(
      allFingersContext, FingerFlexion.Open),
    new FingertipDistanceRelation(
      allFingersContext, RelativeDistance.NotTouching));
            var pianoindex = new HandPose("pianoindex");
            pianoindex.PoseConstraints.Add(new AnyFingerContext(new[] { Finger.Index, }), PoseDirection.Down);
            _pianoindex = new Gesture("Pianoindex", startingPose1, startPose1, pianoindex);







            HandPose startingPose2 = new HandPose("startingPose2", startingPose.PoseConstraints.ToArray());

            var startPose2 = new HandPose(
           "startMiddle", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
           new FingerPose(
             allFingersContext, FingerFlexion.Open),
           new FingertipDistanceRelation(
             allFingersContext, RelativeDistance.NotTouching));
            var pianomiddle = new HandPose("pianomiddle");
            pianomiddle.PoseConstraints.Add(new AnyFingerContext(new[] { Finger.Middle, }), PoseDirection.Down);
            _pianomiddle = new Gesture("Pianomiddle", startingPose2, startPose2, pianomiddle);



            HandPose startingPose3 = new HandPose("startingPose3", startingPose.PoseConstraints.ToArray());

            var startPose3 = new HandPose(
             "startRing", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
             new FingerPose(
               allFingersContext, FingerFlexion.Open),
             new FingertipDistanceRelation(
               allFingersContext, RelativeDistance.NotTouching));
            var pianoring = new HandPose("pianoring");
            pianoring.PoseConstraints.Add(new AnyFingerContext(new[] { Finger.Ring, }), PoseDirection.Down);
            _pianoring = new Gesture("Pianoring", startingPose3, startPose3, pianoring);


            HandPose startingPose4 = new HandPose("startingPose4", startingPose.PoseConstraints.ToArray());

            var startPose4 = new HandPose(
             "startPinky", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
             new FingerPose(
               allFingersContext, FingerFlexion.Open),
             new FingertipDistanceRelation(
               allFingersContext, RelativeDistance.NotTouching));
            var pianopinky = new HandPose("pianopinky");
            pianopinky.PoseConstraints.Add(new AnyFingerContext(new[] { Finger.Pinky, }), PoseDirection.Down);
            _pianopinky = new Gesture("Pianopinky", startingPose4, startPose4, pianopinky);


            HandPose startingPose0 = new HandPose("startingPose0", startingPose.PoseConstraints.ToArray());

            var startPose0 = new HandPose(
             "startThumb", new PalmPose(new AnyHandContext(), PoseDirection.Down, orientation: PoseDirection.Forward),
             new FingerPose(
               allFingersContext, FingerFlexion.Open),
             new FingertipDistanceRelation(
               allFingersContext, RelativeDistance.NotTouching));
            var pianothumb = new HandPose("pianothumb");
            pianothumb.PoseConstraints.Add(new AnyFingerContext(new[] { Finger.Thumb, }), PoseDirection.Down);
            _pianothumb = new Gesture("Pianothumb", startingPose0, startPose0, pianothumb);


            HandPose startingPose00 = new HandPose("startingPose00", startingPose.PoseConstraints.ToArray());
            _start2Pose = new Gesture("start2Pose", startingPose00, start2Pose);




            _swipeleft = GenerateSwipeGesure("SwipeLeft", PoseDirection.Left);
            _swipeleft.Triggered += (s, args) => swipeleft();
            await _gesturesService.RegisterGesture(_swipeleft);


            _swipeup = GenerateSwipeGesure("SwipeUP", PoseDirection.Left);
            _swipeup.Triggered += (s, args) => swipeup();
            await _gesturesService.RegisterGesture(_swipeup);


            _swipedown = GenerateSwipeGesure("SwipeDown", PoseDirection.Left);
            _swipedown.Triggered += (s, args) => swipedown();
            await _gesturesService.RegisterGesture(_swipedown);



            _swiperight = GenerateSwipeGesure("SwipeRight", PoseDirection.Left);
            _swiperight.Triggered += (s, args) =>swiperight();
            await _gesturesService.RegisterGesture(_swiperight);



















            Switzlyphone.Properties.Settings.Default.Save();

            if (cbHands.Checked)
            {

                _gesturesService.ConnectAsync().Wait();

                _start2Pose.Triggered += (s, args) => startpose(button3);

                _pianoindex.Triggered += (s, args) => pianoIndex(button3);
                _pianomiddle.Triggered += (s, args) => pianoMiddle(button3);
                _pianoring.Triggered += (s, args) => pianoRing(button3);
                _pianopinky.Triggered += (s, args) => pianoPinky(button3);
                _pianothumb.Triggered += (s, args) => pianoThumb(button3);

                _gesturesService.RegisterGesture(_start2Pose);
                _gesturesService.RegisterGesture(_pianoindex);
                _gesturesService.RegisterGesture(_pianomiddle);
                _gesturesService.RegisterGesture(_pianoring);
                _gesturesService.RegisterGesture(_pianopinky);
                _gesturesService.RegisterGesture(_pianothumb);
            }


            if (!cbHands.Checked)
            {


                _gesturesService.UnregisterGesture(_start2Pose);


                _gesturesService.UnregisterGesture(_pianoindex);
                _gesturesService.UnregisterGesture(_pianomiddle);
                _gesturesService.UnregisterGesture(_pianoring);
                _gesturesService.UnregisterGesture(_pianopinky);
                _gesturesService.UnregisterGesture(_pianothumb);

                _gesturesService.Disconnect().Wait();

            }
        }

        private void swipeleft()
        {
           

        }
        private void swipeup()
        {


        }
        private void swipedown()
        {


        }
        private void swiperight()
        {


        }
        public void startpose(Button ctrl)
        {
            if (Switzlyphone.Properties.Settings.Default.startpose == 0)
            {

                Switzlyphone.Properties.Settings.Default.startpose = 1;
                Switzlyphone.Properties.Settings.Default.index = 0;
                Switzlyphone.Properties.Settings.Default.middle = 0;
                Switzlyphone.Properties.Settings.Default.ring = 0;
                Switzlyphone.Properties.Settings.Default.pinky = 0;
                Switzlyphone.Properties.Settings.Default.thumb = 0;


            }
            else
            {
                Switzlyphone.Properties.Settings.Default.startpose = 0;

            }
            Switzlyphone.Properties.Settings.Default.Save();
        }
        public void pianoIndex(Button ctrl)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (Switzlyphone.Properties.Settings.Default.index == 0 && Switzlyphone.Properties.Settings.Default.startpose == 1)
            {

                Switzlyphone.Properties.Settings.Default.fingerdown = "index";
                Switzlyphone.Properties.Settings.Default.Save();
                pianoIndex2(ctrl);
                if (ControlInvokeRequired(ctrl, () => clickbtn(ctrl))) return;
                ctrl.PerformClick();


            }
        }
        public void pianoIndex2(Button ctrl)
        {
            if (ControlInvokeRequired(cmbTranspose, () => cmbTranspose.SelectedIndex = 4)) return;
            cmbTranspose.SelectedIndex = 4;
        }
        public void pianoMiddle2(Button ctrl)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(cmbTranspose, () => cmbTranspose.SelectedIndex = 6)) return;
            cmbTranspose.SelectedIndex = 6;
        }
        public void pianoMiddle(Button ctrl)
        {
            if (Switzlyphone.Properties.Settings.Default.middle == 0 && Switzlyphone.Properties.Settings.Default.startpose == 1)
            {

                Switzlyphone.Properties.Settings.Default.fingerdown = "middle";
                Switzlyphone.Properties.Settings.Default.Save();


                pianoMiddle2(ctrl);
                if (ControlInvokeRequired(ctrl, () => clickbtn(ctrl))) return;

                ctrl.PerformClick();


            }
        }
        public void pianoRing2(Button ctrl)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(cmbTranspose, () => cmbTranspose.SelectedIndex = 7)) return;
            cmbTranspose.SelectedIndex = 7;
        }
        public void pianoRing(Button ctrl)
        {
            if (Switzlyphone.Properties.Settings.Default.ring == 0 && Switzlyphone.Properties.Settings.Default.startpose == 1)
            {


                Switzlyphone.Properties.Settings.Default.fingerdown = "ring";
                Switzlyphone.Properties.Settings.Default.Save();
                pianoRing2(ctrl);
                if (ControlInvokeRequired(button3, () => clickbtn(button3))) return;

                button3.PerformClick();

            }
        }
        public void pianoPinky2(Button ctrl)
        {

            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(cmbTranspose, () => cmbTranspose.SelectedIndex = 9)) return;
            cmbTranspose.SelectedIndex = 9;
        }
        public void pianoPinky(Button ctrl)
        {
            if (Switzlyphone.Properties.Settings.Default.pinky == 0 && Switzlyphone.Properties.Settings.Default.startpose == 1)
            {

                Switzlyphone.Properties.Settings.Default.fingerdown = "pinky";

                Switzlyphone.Properties.Settings.Default.Save();
                pianoPinky2(ctrl);
                if (ControlInvokeRequired(button3, () => clickbtn(button3))) return;

                button3.PerformClick();

            }
        }
        public void pianoThumb2(Button ctrl)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(cmbTranspose, () => cmbTranspose.SelectedIndex = 2)) return;
            cmbTranspose.SelectedIndex = 2;
        }
        public void pianoThumb(Button ctrl)
        {
            if (Switzlyphone.Properties.Settings.Default.thumb == 0 && Switzlyphone.Properties.Settings.Default.startpose == 1)
            {

                Switzlyphone.Properties.Settings.Default.fingerdown = "thumb";
                Switzlyphone.Properties.Settings.Default.Save();
                pianoThumb2(ctrl);
                if (ControlInvokeRequired(ctrl, () => clickbtn(ctrl))) return;
                ctrl.PerformClick();

            }
        }
        public void clickbtn(Button ctrl)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(ctrl, () => clickbtn(ctrl))) return;
            ctrl.PerformClick();
        }




        public async void GestureinitAsync()
        {
            _gesturesService = GesturesServiceEndpointFactory.Create();



        }
        private void cbSkeleton_CheckedChanged(object sender, EventArgs e)
        {

            if (!cbSkeleton.Checked)
            {
                cbBodies.Enabled = false;
                cbBody.Enabled = false;
                cbMode.Enabled = false;
                _reader.MultiSourceFrameArrived -= Reader_MultiSourceFrameArrived;
                _reader.Dispose();
                _reader = null;
                return;
            }
            if (cbSkeleton.Checked)
            {
                cbBodies.Enabled = true;
                cbBody.Enabled = true;
                cbMode.Enabled = true;
                emotionStatusLabel.Visible = false;
                emotionLabel.Visible = false;
                rightHandLabel.Visible = false;
                rightHandStatusLabel.Visible = false;
                leftHandLabel.Visible = false;



                _sensor = this.kinectSensor;

                if (_sensor != null)
                {
                    _sensor.Open();

                    _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body | FrameSourceTypes.LongExposureInfrared | FrameSourceTypes.BodyIndex);
                    _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                }
            }

        }

        private BitmapSource ToBitmap2(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            System.Windows.Media.PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((PixelFormats.Bgr32.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;
            BitmapSource bitmap = BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
            ScaleTransform scale = new ScaleTransform((640.0 / bitmap.PixelWidth), (480.0 / bitmap.PixelHeight));
            TransformedBitmap tbBitmap = new TransformedBitmap(bitmap, scale);
            scale = null;
            bitmap = null;
            pixels = null;
            frame.Dispose();
            return tbBitmap;
        }







        private void updateBitmap(int width, int height, System.Drawing.Imaging.PixelFormat pixelFormat, byte[] data)
        {
            this.colorImageBitmap = new Bitmap(width, height, pixelFormat);
            BitmapData bmpData = this.colorImageBitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, this.colorImageBitmap.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride * this.colorImageBitmap.Height;

            Marshal.Copy(data, 0, ptr, bytes);
            this.colorImageBitmap.UnlockBits(bmpData);
        }
        private void updateBitmap(int width, int height, ushort[] rawData, bool colored)
        {
            this.colorImageBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData bmpData = this.colorImageBitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, this.colorImageBitmap.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride * this.colorImageBitmap.Height;

            if (colored)
            {
                byte[] data = new byte[rawData.Length * 4];
                int pixel = 0;
                for (int i = 0; i < rawData.Length; i += 1)
                {
                    int value = rawData[i];
                    int widthTotal = 5;
                    int heightTotal = 5;
                    int maxArea = 8;
                    if (value == -1)
                    {
                        int temp = 0;
                        int tempTotal = 0;
                        for (int y = 0; y < heightTotal; y += 1)
                        {
                            for (int x = 0; x < widthTotal; x += 1)
                            {
                                if (tempTotal > maxArea || i + (width * -y) - x < 0 || i + (width * y) + x >= rawData.Length)
                                {
                                    break;
                                }

                                if (rawData[i + (width * y) + x] != 0)
                                {
                                    temp += rawData[i + width * y + x];
                                    tempTotal += 1;
                                }
                                if (rawData[i + (width * y) - x] != 0)
                                {
                                    temp += rawData[i + width * y + x];
                                    tempTotal += 1;
                                }
                                if (rawData[i + (width * -y) + x] != 0)
                                {
                                    temp += rawData[i + width * y + x];
                                    tempTotal += 1;
                                }
                                if (rawData[i + (width * -y) - x] != 0)
                                {
                                    temp += rawData[i + width * y + x];
                                    tempTotal += 1;
                                }
                            }
                            if (tempTotal > maxArea)
                            {
                                break;
                            }
                        }
                        if (tempTotal > 0)
                        {
                            value = (temp / tempTotal);
                            rawData[i] = (byte)value;
                        }
                    }
                    byte b = (byte)(255 - (value >> 6));
                    byte g = (byte)(255 - (value >> 4));
                    byte r = (byte)(255 - (value >> 2));
                    data[pixel++] = b;
                    data[pixel++] = g;
                    data[pixel++] = r;
                    data[pixel++] = 255;
                }

                Marshal.Copy(data, 0, ptr, bytes);
            }
            else
            {
                byte[] data = new byte[rawData.Length * 4];
                int pixel = 0;
                for (int i = 0; i < rawData.Length; i += 1)
                {
                    ushort currentRaw = rawData[i];
                    ushort oldRaw = irImagePixelDataOld[i];
                    int rawValue = Math.Abs(rawData[i] - irImagePixelDataOld[i]);
                    if (rawValue > 1000)
                    {
                        byte intensity = (byte)(rawValue >> 8);

                        data[pixel++] = 0;
                        data[pixel++] = 0;
                        data[pixel++] = 255;
                    }
                    else
                    {
                        byte intensity = (byte)(rawValue >> 8);

                        data[pixel++] = intensity;
                        data[pixel++] = 0;
                        data[pixel++] = 0;
                    }

                    data[pixel++] = 255;
                }

                Marshal.Copy(data, 0, ptr, bytes);
            }

            this.colorImageBitmap.UnlockBits(bmpData);
        }




        void MultiSourceFrameReader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {

            if (cbFancy.Checked)
            {

                var reference = e.FrameReference.AcquireFrame();



                //    reference.coordinateMapper.MapDepthFrameToCameraSpace(depthData, camerapoints);
                //   this.coordinateMapper.MapDepthFrameToColorSpace(depthData, colorpoints);
                // Color






                MultiSourceFrameReference msFrameReference = e.FrameReference;

                try
                {
                    MultiSourceFrame msFrame = msFrameReference.AcquireFrame();

                    if (msFrame != null)
                    {
                        LongExposureInfraredFrameReference leirFrameReference = msFrame.LongExposureInfraredFrameReference;
                        InfraredFrameReference irFrameReference = msFrame.InfraredFrameReference;
                        ColorFrameReference colorFrameReference = msFrame.ColorFrameReference;
                        DepthFrameReference depthFrameReference = msFrame.DepthFrameReference;
                        BodyFrameReference bodyFrameReference = msFrame.BodyFrameReference;
                        /*    switch (this.imageType)
                            {
                                case ImageType.Color:

                                    using (var frame4 = msFrame.ColorFrameReference.AcquireFrame())
                                    {
                                        if (frame4 != null)
                                        {

                                            camera.Image = ImageWpfToGDI(ToBitmap2(frame4));
                                        }
                                        //   useColorFrame(colorFrameReference);
                                    }
                                    break;

                                case ImageType.Depth:
                                    useDepthFrame(depthFrameReference);
                                    break;
                                case ImageType.IR:

                                    using (var frame3 = msFrame.InfraredFrameReference.AcquireFrame())
                                    {
                                        if (frame3 != null)
                                        {


                                            camera.Image = ImageWpfToGDI(frame3.ToBitmap());



                                        }
                                    }
                                         //   useIRFrame(irFrameReference);
                                                break;
                                case ImageType.LEIR:
                                    useLIRFrame(leirFrameReference);
                                    break;
                            }
                            */
                        useBodyFrame(bodyFrameReference);
                        updatePulse(colorFrameReference, irFrameReference, bodyFrameReference);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        void updatePulse(ColorFrameReference colorFrameReference, InfraredFrameReference irFrameReference, BodyFrameReference bodyFrameReference)
        {
            long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            int width = 0;
            int height = 0;

            try
            {
                if (this.bodyToTrack > -1)
                {
                    BodyFrame bodyFrame = bodyFrameReference.AcquireFrame();

                    if (bodyFrame != null)
                    {
                        using (bodyFrame)
                        {
                            bodyFrame.GetAndRefreshBodyData(this.bodies);

                            Body body = this.bodies[this.bodyToTrack];
                            if (body.IsTracked)
                            {
                                CameraSpacePoint headPosition = body.Joints[JointType.Head].Position;
                                CameraSpacePoint neckPosition = body.Joints[JointType.Neck].Position;

                                float centerX = neckPosition.X - headPosition.X;
                                centerX = headPosition.X + (centerX / 2.0f);

                                float centerY = neckPosition.Y - headPosition.Y;
                                centerY = headPosition.Y + (centerY / 2.0f);

                                centerX += 1.0f;
                                centerX /= 2.0f;

                                centerY += 1.0f;
                                centerY /= 2.0f;

                                if (this.colorImageBitmap != null)
                                {
                                    System.Drawing.Color c = this.colorImageBitmap.GetPixel((int)(centerX * this.colorImageBitmap.Width), (int)(centerY * this.colorImageBitmap.Height));

                                    hueValues.Enqueue(c.GetHue());
                                    if (hueValues.Count > 10)
                                    {
                                        hueValues.Dequeue();
                                    }

                                    if (hueValues.Count >= 10)
                                    {
                                        //this.pulseLabel.Text = "Pulse: " + ((float)c.GetHue() / (float)hueValues.Average());
                                        if (c.GetHue() > hueValues.Average())
                                        {
                                            pulseLabel.Text = "Pulse: " + ((float)(currentTime - lastTime) / (float)pulses);
                                            //this.pulseLabel.Text = "Pulse: 1";
                                            pulses += 1;
                                        }
                                        if (currentTime - lastTime > 1000 * 5)
                                        {
                                            lastTime = currentTime;
                                            pulses = 0;
                                        }
                                        Console.WriteLine("Hue Average: " + hueValues.Average());
                                    }
                                }

                                if (width > 0 && height > 0)
                                {
                                    ushort irValue = this.irImagePixelData[(int)(centerX * width) + (int)(centerY * height) * width];

                                    irValues.Enqueue(irValue);
                                    if (irValues.Count > 10)
                                    {
                                        irValues.Dequeue();
                                    }

                                    if (irValues.Count >= 10)
                                    {
                                        //Console.WriteLine("IR Average: " + irValues.Average());
                                    }
                                }

                                //Console.WriteLine("Color: " + c.R + ", " + c.G + ", " + c.B);
                                //Console.WriteLine("Position:" + centerX + ", " + centerY);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Console.WriteLine(message);
                // Don't worry about empty frames.
            }
        }

        void useColorFrame(ColorFrameReference colorFrameReference)
        {

            try
            {
                ColorFrame colorFrame = colorFrameReference.AcquireFrame();

                if (colorFrame != null)
                {
                    using (colorFrame)
                    {
                        if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                        {
                            colorFrame.CopyRawFrameDataToArray(this.colorImagePixelData);
                        }
                        else
                        {
                            colorFrame.CopyConvertedFrameDataToArray(this.colorImagePixelData, ColorImageFormat.Bgra);
                        }

                        this.updateBitmap(colorFrame.FrameDescription.Width, colorFrame.FrameDescription.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb, this.colorImagePixelData);

                        this.camera.Image = ImageWpfToGDI(ToBitmap2(colorFrame));
                    }
                }
            }
            catch (Exception er)
            {
                string message = er.Message;
                Console.WriteLine(message);
                // Don't worry about empty frames.
            }
        }
        void useDepthFrame(DepthFrameReference depthFrameReference)
        {
            try
            {
                DepthFrame depthFrame = depthFrameReference.AcquireFrame();

                if (depthFrame != null)
                {
                    using (depthFrame)
                    {
                        depthFrame.CopyFrameDataToArray(this.irImagePixelData);

                        this.updateBitmap(depthFrame.FrameDescription.Width, depthFrame.FrameDescription.Height, this.irImagePixelData, true);

                        this.camera.Image = new Bitmap(this.colorImageBitmap, this.camera.Width, this.camera.Height);
                    }
                }
            }
            catch (Exception er)
            {
                string message = er.Message;
                Console.WriteLine(message);
                // Don't worry about empty frames.
            }
        }
        void useIRFrame(InfraredFrameReference irFrameReference)
        {
            try
            {
                InfraredFrame IRFrame = irFrameReference.AcquireFrame();

                if (IRFrame != null)
                {
                    using (IRFrame)
                    {
                        IRFrame.CopyFrameDataToArray(this.irImagePixelData);

                        this.updateBitmap(IRFrame.FrameDescription.Width, IRFrame.FrameDescription.Height, this.irImagePixelData, false);

                        IRFrame.CopyFrameDataToArray(this.irImagePixelDataOld);

                        this.camera.Image = new Bitmap(this.colorImageBitmap, this.camera.Width, this.camera.Height);
                    }
                }
            }
            catch (Exception er)
            {
                string message = er.Message;
                Console.WriteLine(message);
                // Don't worry about empty frames.
            }
        }
        void useLIRFrame(LongExposureInfraredFrameReference leIRFrameReference)
        {
            try
            {
                LongExposureInfraredFrame leIRFrame = leIRFrameReference.AcquireFrame();

                if (leIRFrame != null)
                {
                    using (leIRFrame)
                    {
                        leIRFrame.CopyFrameDataToArray(this.irLEImagePixelData);

                        this.updateBitmap(leIRFrame.FrameDescription.Width, leIRFrame.FrameDescription.Height, this.irLEImagePixelData, false);

                        this.camera.Image = new Bitmap(this.colorImageBitmap, this.camera.Width, this.camera.Height);
                    }
                }
            }
            catch (Exception er)
            {
                string message = er.Message;
                Console.WriteLine(message);
                // Don't worry about empty frames.
            }
        }
        void useBodyFrame(BodyFrameReference bodyFrameReference)
        {
            try
            {
                BodyFrame bodyFrame = bodyFrameReference.AcquireFrame();

                if (bodyFrame != null)
                {
                    using (bodyFrame)
                    {
                        bodyFrame.GetAndRefreshBodyData(this.bodies);
                        if (this.bodyToTrack < 0)
                        {
                            this.findBodyToTrack();
                        }

                        if (this.bodyToTrack > -1)
                        {
                            this.updateCurrentBody();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Console.WriteLine(message);
                // Don't worry about empty frames.
            }
        }


        private void findBodyToTrack()
        {
            for (int i = 0; i < this.bodyCount; i += 1)
            {
                if (this.bodies[i].IsTracked)
                {
                    if (this.startTrackingGesture(this.bodies[i]))
                    {
                        this.bodyToTrack = i;
                        break;
                    }
                }
            }
        }
        private bool startTrackingGesture(Body body)
        {
            //if (body.Activities[Activity.LookingAway] == DetectionResult.No)
            {
                if (body.HandLeftState == HandState.Lasso && body.HandRightState == HandState.Lasso)
                {
                    return true;
                }
            }
            return false;
        }

        private void updateCurrentBody()
        {
            Body body = this.bodies[this.bodyToTrack];

            if (body.IsTracked)
            {
                if (stopTrackingGesture(body))
                {

                    this.leftHandStausLabel.Text = "Not Tracking";
                    this.rightHandStatusLabel.Text = "Not Tracking";
                    this.emotionStatusLabel.Text = "Not Tracking";
                    updateRadar(null);
                    this.bodyToTrack = -1;
                }
                else
                {
                    this.updateEyes(body);
                    this.updateMouth(body);
                    this.updateHands(body);
                    this.updateEmotions(body);
                    this.updateRadar(body);
                }
            }
            else
            {
                this.bodyToTrack = -1;
            }
        }
        private bool stopTrackingGesture(Body body)
        {
            // if (body.Activities[Activity.LookingAway] == DetectionResult.Yes)
            {
                if (body.HandLeftState == HandState.Closed && body.HandRightState == HandState.Closed)
                {
                    return true;
                }
            }
            return false;
        }

        private void updateEyes(Body body)
        {
            switch (Microsoft.Kinect.Face.FaceProperty.LeftEyeClosed)
            {

                case Microsoft.Kinect.Face.FaceProperty.LeftEyeClosed:

                    break;


            }
            switch (Microsoft.Kinect.Face.FaceProperty.RightEyeClosed)
            {

                case Microsoft.Kinect.Face.FaceProperty.RightEyeClosed:

                    break;
            }
        }
        private void updateMouth(Body body)
        {
            switch (Microsoft.Kinect.Face.FaceProperty.MouthOpen)
            {
                case Microsoft.Kinect.Face.FaceProperty.MouthOpen:

                    break;

            }
        }
        private void updateHands(Body body)
        {
            switch (body.HandLeftState)
            {
                case HandState.Closed:
                    this.leftHandStausLabel.Text = "Closed";
                    break;
                case HandState.Open:
                    this.leftHandStausLabel.Text = "Open";
                    break;
                case HandState.Lasso:
                    this.leftHandStausLabel.Text = "Lasso";
                    break;
                case HandState.NotTracked:
                    this.leftHandStausLabel.Text = "Not Tracked";
                    break;
            }
            switch (body.HandRightState)
            {
                case HandState.Closed:
                    this.rightHandStatusLabel.Text = "Closed";
                    break;
                case HandState.Open:
                    this.rightHandStatusLabel.Text = "Open";
                    break;
                case HandState.Lasso:
                    this.rightHandStatusLabel.Text = "Lasso";
                    break;
                case HandState.NotTracked:
                    this.rightHandStatusLabel.Text = "Not Tracked";
                    break;
            }
        }




        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Body[] bodies = new Body[frame.BodyCount];
                    frame.GetAndRefreshBodyData(bodies);

                    Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                    if (!_faceSource.IsTrackingIdValid)
                    {
                        if (body != null)
                        {
                            _faceSource.TrackingId = body.TrackingId;

                        }
                    }
                }
            }
        }
        private void FaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null && frame.IsFaceTracked)
                {

                    frame.GetAndRefreshFaceAlignmentResult(_faceAlignment);
                    UpdateFacePoints();

                }
            }
        }



        private void UpdateFacePoints()
        {
            if (_faceModel == null) return;

            var vertices = _faceModel.CalculateVerticesForAlignment(_faceAlignment);

            if (vertices.Count > 0)
            {
                if (_points.Count == 0)
                {
                    for (int index = 0; index < vertices.Count; index++)
                    {
                        Ellipse ellipse = new Ellipse
                        {
                            Width = 2.0,
                            Height = 2.0,
                            Fill = new SolidColorBrush(Colors.Blue)
                        };

                        _points.Add(ellipse);
                    }

                    foreach (Ellipse ellipse in _points)
                    {
                        //          canvas.Children.Add(ellipse);
                    }
                }

                for (int index = 0; index < vertices.Count; index++)
                {
                    CameraSpacePoint vertice = vertices[index];
                    DepthSpacePoint point = _sensor.CoordinateMapper.MapCameraPointToDepthSpace(vertice);

                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) return;

                    Ellipse ellipse = _points[index];

                    //        Canvas.SetLeft(ellipse, point.X);
                    //        Canvas.SetTop(ellipse, point.Y);
                }
            }
        }


        private void updateEmotions(Body body)
        {

            //      switch (Microsoft.Kinect.Face.FaceProperty)
            //      {
            //       case Microsoft.Kinect.Face.FaceProperty.Happy:
            //          this.emotionStatusLabel.Text = "Happy";
            ///         break;
            //     case Microsoft.Kinect.Face.FaceProperty.WearingGlasses:
            //         this.emotionStatusLabel.Text = "Glasses off!";
            ////         break;
            //     case Microsoft.Kinect.Face.FaceProperty.Engaged:
            //        this.emotionStatusLabel.Text = "Engaged!!";
            //           break;
            //    case Microsoft.Kinect.Face.FaceProperty.LookingAway:
            //      this.emotionStatusLabel.Text = "Looking away!!";
            //       break;
            //   }

        }
        private void updateRadar(Body body)
        {
            System.Drawing.Point position = new System.Drawing.Point(70, 70);
            if (body != null)
            {
                position = new System.Drawing.Point((int)((body.Joints[JointType.SpineMid].Position.X + 1) * 150 / 2), (int)(body.Joints[JointType.SpineMid].Position.Z * 150 / 6));
            }
            // this.personRadar1.setPosition(position);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.multiSourceFrameReader != null)
            {
                this.multiSourceFrameReader.Dispose();
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
            }

            if (this.colorImageBitmap != null)
            {
                this.colorImageBitmap.Dispose();
            }

            this.camera.Dispose();
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            imageType = ImageType.Color;
        }

        private void depthButton_Click(object sender, EventArgs e)
        {
            imageType = ImageType.Depth;
        }

        private void irButton_Click(object sender, EventArgs e)
        {
            imageType = ImageType.IR;
        }

        private void btir_Click(object sender, EventArgs e)
        {
            imageType = ImageType.LEIR;
        }

        private void takePicButton_Click(object sender, EventArgs e)
        {

        }



        public enum ImageType
        {
            Color = 0,
            Depth = 1,
            IR = 2,
            LEIR = 3
        }

        private void colorButton_Click_1(object sender, EventArgs e)
        {
            imageType = ImageType.Color;
        }

        private void depthButton_Click_1(object sender, EventArgs e)
        {
            imageType = ImageType.Depth;
        }

        private void irButton_Click_1(object sender, EventArgs e)
        {
            imageType = ImageType.IR;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageType = ImageType.LEIR;
        }

        private void takePicButton_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "png files (*.png)|*.png";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.camera.Image.Save(dialog.FileName);
            }
        }

        private void leftHandStausLabel_Click(object sender, EventArgs e)
        {

        }

        private void cbFancy_CheckedChanged(object sender, EventArgs e)
        {

            if (_sensor != null)
            {
                // Listen for body data.
                _bodySource = _sensor.BodyFrameSource;
                _bodyReader = _bodySource.OpenReader();
                _bodyReader.FrameArrived += BodyReader_FrameArrived;

                // Listen for HD face data.
                _faceSource = new HighDefinitionFaceFrameSource(_sensor);
                _faceReader = _faceSource.OpenReader();
                _faceReader.FrameArrived += FaceReader_FrameArrived;

                _faceModel = new FaceModel();
                _faceAlignment = new FaceAlignment();

                // Start tracking!        
                _sensor.Open();
            }
            if (cbFancy.Checked)
            {
                {

                    emotionStatusLabel.Visible = true;
                    emotionLabel.Visible = true;
                    rightHandLabel.Visible = true;
                    rightHandStatusLabel.Visible = true;
                    leftHandLabel.Visible = true;
                    leftHandStausLabel.Visible = true;


                    this.multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameReader_MultiSourceFrameArrived;



                }
                if (!cbFancy.Checked)
                {
                    this.multiSourceFrameReader.MultiSourceFrameArrived -= MultiSourceFrameReader_MultiSourceFrameArrived;
                    this.multiSourceFrameReader.Dispose();

                    emotionStatusLabel.Visible = false;
                    emotionLabel.Visible = false;
                    rightHandLabel.Visible = false;
                    rightHandStatusLabel.Visible = false;
                    leftHandLabel.Visible = false;
                    leftHandStausLabel.Visible = false;


                    kinectSensor.Close();
                    kinectSensor = null;


                    this.irImagePixelData = null;
                    irImagePixelDataOld = null;
                    this.colorImagePixelData = null;
                    this.bodies = null;
                    this.bodyCount = 0;
                    this.irLEImagePixelData = null;
                    this.multiSourceFrameReader = null;


                    //   GC.Collect();
                    //   GC.WaitForPendingFinalizers();
                    //   GC.Collect();


                }
            }
        }

        private void facialDiagram1_Load(object sender, EventArgs e)
        {

        }







        internal static class ApplicationSettings
        {

            //added public static because I didn't see how you planned on invoking save
            public static void Save(int no, string value)
            {
                //sets the nameX
                Switzlyphone.Properties.Settings.Default[value] = no;
                //save the settings
                Switzlyphone.Properties.Settings.Default.Save();
            }
            public static int Load(string no)
            {

                //sets the nameX
                int value = Convert.ToInt16(Switzlyphone.Properties.Settings.Default[no]);
                //    System.Windows.MessageBox.Show(value.ToString());
                return value;


            }

        }
        private Gesture GenerateSwipeGesure(string name, PoseDirection direction)
        {
            var fingers = new[] { Finger.Index, Finger.Middle, Finger.Ring };
            var fingerSet = new HandPose("FingersSet", new PalmPose(new AnyHandContext(), direction, orientation: PoseDirection.Forward),
                                                       new FingertipDistanceRelation(Finger.Middle, RelativeDistance.Touching, new[] { Finger.Index, Finger.Ring }),
                                                       new FingerPose(fingers, PoseDirection.Forward));

            var fingersBent = new HandPose("FingersBent", new PalmPose(new AnyHandContext(), direction, orientation: PoseDirection.Forward),
                                                          new FingertipDistanceRelation(Finger.Middle, RelativeDistance.Touching, new[] { Finger.Index, Finger.Ring }),
                                                          new FingerPose(fingers, direction | PoseDirection.Backward));

            var swipeGesture = new Gesture(name, fingerSet, fingersBent);
            return swipeGesture;
        }
    
    private HandPose GenerateOpenFingersPose(string name, Finger[] fingers)
        {
            var nonThumbOtherFingers = (new[] { Finger.Index, Finger.Middle, Finger.Ring, Finger.Pinky }).Except(fingers);
            var fingersOpenPose = new HandPose(name, new PalmPose(new AnyHandContext(), PoseDirection.Down, PoseDirection.Down),
                                                     new FingerPose(fingers, FingerFlexion.Open, PoseDirection.Down),
                                                     new FingertipDistanceRelation(fingers, RelativeDistance.NotTouching, nonThumbOtherFingers.Union(new[] { Finger.Thumb })));
            if (nonThumbOtherFingers.Any()) fingersOpenPose.PoseConstraints.Add(new FingerPose(nonThumbOtherFingers, PoseDirection.Down | PoseDirection.Backward | PoseDirection.Forward | PoseDirection.Left | PoseDirection.Right));
            return fingersOpenPose;
        }


    }

}
