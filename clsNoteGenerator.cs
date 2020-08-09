using System;

namespace MusicMaker
{
    internal class clsNoteGenerator
    {
        public int speed_beatsPerMinute = 90;
        public int macro_numCrotchets = 8 * 4; // plays a minim note every this many crotchets

        public float chanceOfSemibreveNote = 0.02f; // whole note
        public float chanceOfMinimNote = 0.05f;
        public float chanceOfCrotchetNote = 0.1f;
        public float chanceOfMultipleItem_Triplet = 0.05f;
        public float chanceOfMultipleItem_Quaver = 0.2f;
        public float chanceOfMultipleItem_Semiquaver = 0.0f;
        public float chanceOfSemibreveRest = 0.0f;
        public float chanceOfMinimRest = 0.025f;
        public float chanceOfCrotchetRest = 0.025f;

        // all above group are probabilities used in ratio to their total sum
        public float quaverSwingPercent = 66;

        public float chanceOfTie = 0.1f; // out of 1
        public float chanceOfTieInMultipleItem_Triplet = 0.1f;
        public float chanceOfNoteInMultipleItem_Triplet = 0.6f;
        public float chanceOfTieInMultipleItem_Quaver = 0.1f;
        public float chanceOfNoteInMultipleItem_Quaver = 0.5f;
        public float chanceOfTieInMultipleItem_Semiquaver = 0.1f;
        public float chanceOfNoteInMultipleItem_Semiquaver = 0.8f;

        public float chanceOfPitchChange0ScaleNotes = 0.0f;
        public float chanceOfPitchChange1ScaleNotes = 0.4f;
        public float chanceOfPitchChange2ScaleNotes = 0.2f;
        public float chanceOfPitchChange3ScaleNotes = 0.1f;
        public float chanceOfPitchChange4ScaleNotes = 0.1f;
        public float chanceOfPitchChange5ScaleNotes = 0.1f;
        public float chanceOfPitchChange6ScaleNotes = 0.05f;
        public float chanceOfPitchChange7ScaleNotes = 0.05f;
        // all chanceOfPitchChange..ScaleNotes above are probabilities used in ratio to their total sum
        // change of increase vs decrease in pitch always 0.5

        // use dorian scale for now
        public int[] scalePositionToPitchArr = new int[7] { 0, 2, 3, 5, 7, 9, 10 };

        private Random rnd = new Random();

        public int minPitchInSemitones = 40;
        public int middlePitchInSemitones = 60;
        public int maxPitchInSemitones = 100;
        public int transposeOffsetInSemitones = 0;
        public bool forceMiddlePitch = false;
        public int[] forcedPitches = new int[] { }; // random with equal probability
        public bool runSemitonesAscending = false;
        public bool runScaleAscending = false;

        public float chance1SimulaneousNotes = 1.0f;
        public float chance2SimulaneousNotes = 0.0f;
        public float chance3SimulaneousNotes = 0.0f;
        public float chance4SimulaneousNotes = 0.0f;
        public bool constantTotalSimulanousNotes = true;
        public int totalSimultanousNotes = 1;
        private const int maxSimultaneousNotes = 4;
        private int[] lastNoteScalePosition = new int[maxSimultaneousNotes]; // initially 0
        private int[] lastNotePitchInSemitones = new int[maxSimultaneousNotes]; // initially -1;

        public short velocityMinimum = 127;
        public short velocityMaximum = 127;

        private uint startingTick;
        private uint currentComposingTick;
        private int channel;
        private FluidSynthWrapper.Sequencer seq;
        private string musicHistory = "";

        public string getMusicHistory()
        {
            return musicHistory;
        }

        public uint getCurrentComposingTick()
        {
            return currentComposingTick;
        }

        private int crotchetCount()
        {
            return (int)Math.Round((currentComposingTick - startingTick) / timeMS_per_crotchet());
        }

        public void close()
        {
            endLastNote();
        }

        private int checkProbabilityForScaleNote(int direction, ref float rnd1, float chance, float totalChance, int numNotes)
        {
            if (rnd1 < (chance / totalChance))
            {
                return numNotes * direction;
            }
            rnd1 -= (chance / totalChance);
            return 999;
        }

        private int getChangeInScaleNote()
        {
            if (runScaleAscending)
            {
                return 1;
            }
            int direction = Math.Sign(rnd.NextDouble() - 0.5);
            float totalChance = chanceOfPitchChange0ScaleNotes +
                    chanceOfPitchChange1ScaleNotes +
                    chanceOfPitchChange2ScaleNotes +
                    chanceOfPitchChange3ScaleNotes +
                    chanceOfPitchChange4ScaleNotes +
                    chanceOfPitchChange5ScaleNotes +
                    chanceOfPitchChange6ScaleNotes +
                    chanceOfPitchChange7ScaleNotes;
            float rnd1 = (float)rnd.NextDouble();
            int result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange0ScaleNotes, totalChance, 0);
            if (result != 999) return result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange1ScaleNotes, totalChance, 1);
            if (result != 999) return result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange2ScaleNotes, totalChance, 2);
            if (result != 999) return result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange3ScaleNotes, totalChance, 3);
            if (result != 999) return result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange4ScaleNotes, totalChance, 4);
            if (result != 999) return result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange5ScaleNotes, totalChance, 5);
            if (result != 999) return result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange6ScaleNotes, totalChance, 6);
            if (result != 999) return result;
            result = checkProbabilityForScaleNote(direction, ref rnd1, chanceOfPitchChange7ScaleNotes, totalChance, 7);
            if (result != 999) return result;
            return 0;
        }

        private int scalePositionToPitchSemitones(int scalePosition)
        {
            //scalePosition of 0 means no shift, numNotesPerOctave means shift of 12 semitones;
            // anything between 0 and numNotesPerOctave-1 is determined by scalePositionToPitchArr ie it depends on the scale;
            // multiples of numNotesPerOctave notes add octaves by multiples of 12 semitones.
            // transposeOffsetInSemitones is added to the final result.
            int numNotesPerOctave = scalePositionToPitchArr.Length;
            int octavePitchShift = Math.Sign(scalePosition) * 12 * (int)Math.Floor(Math.Abs(scalePosition) / (float)numNotesPerOctave);
            int withinOctavePitchShift = Math.Sign(scalePosition) * scalePositionToPitchArr[(int)Math.Abs(scalePosition % numNotesPerOctave)];
            return octavePitchShift + withinOctavePitchShift + transposeOffsetInSemitones;
        }

        // initializer for class
        public clsNoteGenerator(uint startingTick, FluidSynthWrapper.Sequencer seq, int channel)
        {
            this.startingTick = startingTick;
            currentComposingTick = startingTick;
            this.seq = seq;
            this.channel = channel;
            for (int noteNumber = 0; noteNumber < maxSimultaneousNotes; ++noteNumber)
            {
                lastNoteScalePosition[noteNumber] = 0;
                lastNotePitchInSemitones[noteNumber] = -1;
            }
        }

        private void ensureTotalSimulaneousNotesIsOk()
        {
            if (totalSimultanousNotes > maxSimultaneousNotes)
            {
                totalSimultanousNotes = maxSimultaneousNotes;
            }
            if (totalSimultanousNotes < 1)
            {
                totalSimultanousNotes = 1;
            }
        }

        private void endLastNote()
        {
            ensureTotalSimulaneousNotesIsOk();
            for (int noteNumber = 0; noteNumber < totalSimultanousNotes; ++noteNumber)
            {
                if (lastNotePitchInSemitones[noteNumber] != -1)
                {
                    musicHistory += "NoteOff(" + currentComposingTick.ToString() + "," + lastNotePitchInSemitones[noteNumber].ToString() + "," + lastNoteScalePosition[noteNumber].ToString() + ")" + Environment.NewLine;
                    seq.PlayEventAt(FluidSynthWrapper.Event.NoteOff(channel, (short)lastNotePitchInSemitones[noteNumber]), currentComposingTick, true);
                    lastNotePitchInSemitones[noteNumber] = -1;
                }
            }
            // generate new totalSimultanousNotes value
            // (this needs to be done after all NoteOff's to make sure
            // that we don't end up with a note stuck down that is no
            // longer tracked)
            if (constantTotalSimulanousNotes) return;
            float rnd1 = (float)rnd.NextDouble();
            float totalChance =
                chance1SimulaneousNotes +
                chance2SimulaneousNotes +
                chance3SimulaneousNotes +
                chance4SimulaneousNotes;
            if (rnd1 < chance1SimulaneousNotes / totalChance)
            {
                totalSimultanousNotes = 1;
                return;
            }
            rnd1 -= chance1SimulaneousNotes / totalChance;
            if (rnd1 < chance2SimulaneousNotes / totalChance)
            {
                totalSimultanousNotes = 2;
                return;
            }
            rnd1 -= chance2SimulaneousNotes / totalChance;
            if (rnd1 < chance3SimulaneousNotes / totalChance)
            {
                totalSimultanousNotes = 3;
                return;
            }
            rnd1 -= chance3SimulaneousNotes / totalChance;
            if (rnd1 < chance4SimulaneousNotes / totalChance)
            {
                totalSimultanousNotes = 4;
                return;
            }
            totalSimultanousNotes = 1;
        }

        // if isNote is false, it is a rest
        private void performEvent(float timeMS, bool isNote, bool isTie, short velocity)
        {
            if (runScaleAscending)
            {
                timeMS = timeMS_per_crotchet();
                isNote = true;
                isTie = false;
                velocity = 127;
            }
            if (isTie)
            {
                // if we are currently playing a note
                // (ie lastNotePitchInSemitones[noteNumber] != -1 for any noteNumber),
                // and isNote is true
                // then we simply extend currentComposingTick by timeMS;
                // if we are currently not playing a note,
                // and isNote is false, we also extend currentComposingTick by timeMS
                // (in order to tie two rests).
                // otherwise, we set isTie to false and continue, since
                // we cannot tie a rest to a note or a note to a rest.
                bool currentlyPlayingANote = false;
                for (int noteNumber = 0; noteNumber < totalSimultanousNotes; ++noteNumber)
                {
                    if (lastNotePitchInSemitones[noteNumber] != -1)
                    {
                        currentlyPlayingANote = true;
                    }
                }
                if ((currentlyPlayingANote && isNote) || (!currentlyPlayingANote && !isNote))
                {
                    currentComposingTick += (uint)Math.Round(timeMS);
                    return;
                }
                isTie = false;
            }
            endLastNote();
            if (isNote)
            {
                for (int noteNumber = 0; noteNumber < totalSimultanousNotes; ++noteNumber)
                {
                    int currentScaleNote = lastNoteScalePosition[noteNumber] + getChangeInScaleNote();
                    int currentPitchInSemitones = scalePositionToPitchSemitones(currentScaleNote);
                    if (runScaleAscending && (currentPitchInSemitones > maxPitchInSemitones))
                    {
                        // move to nearest scale note to lowest semitone note (minPitchInSemitones) next
                        int numNotesPerOctave = scalePositionToPitchArr.Length;
                        currentScaleNote = (int)Math.Round(numNotesPerOctave * minPitchInSemitones / 12.0);
                        currentPitchInSemitones = scalePositionToPitchSemitones(currentScaleNote);
                    }
                    if (!runScaleAscending && ((currentPitchInSemitones > maxPitchInSemitones) || (currentPitchInSemitones < minPitchInSemitones)))
                    {
                        // Play a note in the middle next
                        int numNotesPerOctave = scalePositionToPitchArr.Length;
                        currentScaleNote = (int)Math.Round(numNotesPerOctave * middlePitchInSemitones / 12.0);
                        currentPitchInSemitones = scalePositionToPitchSemitones(currentScaleNote);
                    }
                    if (forceMiddlePitch)
                    {
                        // Play a note in the middle next
                        int numNotesPerOctave = scalePositionToPitchArr.Length;
                        currentScaleNote = (int)Math.Round(numNotesPerOctave * middlePitchInSemitones / 12.0);
                        currentPitchInSemitones = middlePitchInSemitones;
                    }
                    if (!forceMiddlePitch && (forcedPitches.Length > 0))
                    {
                        int forcedPitchIdx = rnd.Next(0, forcedPitches.Length);
                        if (forcedPitchIdx > forcedPitches.Length - 1)
                        {
                            forcedPitchIdx = forcedPitches.Length - 1;
                        }
                        currentScaleNote = forcedPitchIdx;
                        currentPitchInSemitones = forcedPitches[forcedPitchIdx];
                    }
                    musicHistory += "NoteOn(" + currentComposingTick.ToString() + "," + currentPitchInSemitones.ToString() + "," + currentScaleNote.ToString() + ")" + Environment.NewLine;
                    seq.PlayEventAt(FluidSynthWrapper.Event.NoteOn(channel, (short)currentPitchInSemitones, velocity), currentComposingTick, true);
                    lastNoteScalePosition[noteNumber] = currentScaleNote;
                    lastNotePitchInSemitones[noteNumber] = currentPitchInSemitones;
                }
            }
            currentComposingTick += (uint)Math.Round(timeMS);
        }

        // a multiple event consists of multiple notes or rests,
        // taking up the total time of 1 crotchet.
        // It is random 50% chance whether
        // each of the items is a note or rest. There is a 10% chance
        // that a tie will occur.
        private bool checkProbabilityForTimingAndRunMultipleEvent(int quantityOfItems, float chanceOfTie, float chanceOfNote, ref float rnd1, float chance, float totalChance, float timeMS, string description, short velocity)
        {
            if (rnd1 < (chance / totalChance))
            {
                bool isNote;
                bool isTie;

                if (quantityOfItems == 2)
                {
                    if (quaverSwingPercent > 100.0f) quaverSwingPercent = 100.0f;
                    if (quaverSwingPercent < 0.0f) quaverSwingPercent = 0.0f;
                    float timeMSFirst = timeMS_per_crotchet() * quaverSwingPercent / 100.0f;
                    float timeMSSecond = timeMS_per_crotchet() - timeMSFirst;

                    // swung quavers. apply quaverSwingPercent
                    isNote = (rnd.NextDouble() < chanceOfNote);
                    isTie = (rnd.NextDouble() < chanceOfTie);
                    musicHistory += "Event to occur: " + description + " swung " + quaverSwingPercent.ToString() + "% (isNote:" + isNote.ToString() + ", isTie:" + isTie.ToString() + ")" + Environment.NewLine;
                    performEvent(timeMSFirst, isNote, isTie, velocity);

                    isNote = (rnd.NextDouble() < chanceOfNote);
                    isTie = (rnd.NextDouble() < chanceOfTie);
                    musicHistory += "Event to occur: " + description + " swung " + quaverSwingPercent.ToString() + "% (isNote:" + isNote.ToString() + ", isTie:" + isTie.ToString() + ")" + Environment.NewLine;
                    performEvent(timeMSSecond, isNote, isTie, velocity);

                    return true;
                }

                for (int itemNumber = 0; itemNumber < quantityOfItems; ++itemNumber)
                {
                    isNote = (rnd.NextDouble() < chanceOfNote);
                    isTie = (rnd.NextDouble() < chanceOfTie);
                    musicHistory += "Event to occur: " + description + " (isNote:" + isNote.ToString() + ", isTie:" + isTie.ToString() + ")" + Environment.NewLine;
                    performEvent(timeMS, isNote, isTie, velocity);
                }
                return true;
            }
            rnd1 -= (chance / totalChance);
            return false;
        }

        // return true if we performed the event
        private bool checkProbabilityForTimingAndRunEvent(bool isTie, bool isNote, ref float rnd1, float chance, float totalChance, float timeMS, string description, short velocity)
        {
            if (rnd1 < (chance / totalChance))
            {
                musicHistory += "Event to occur: " + description + " (isNote:" + isNote.ToString() + ", isTie:" + isTie.ToString() + ")" + Environment.NewLine;
                performEvent(timeMS, isNote, isTie, velocity);
                return true;
            }
            rnd1 -= (chance / totalChance);
            return false;
        }

        public void runNoteOnOffChanges()
        {
            short velocity = (short)rnd.Next(velocityMinimum, velocityMaximum);
            if (runSemitonesAscending)
            {
                float rnd2 = 0.5f;
                forceMiddlePitch = true;
                checkProbabilityForTimingAndRunEvent(false, true, ref rnd2, 1.0f, 1.0f, timeMS_per_crotchet(), "SemitonesAscending-CrotchetNote", velocity);
                middlePitchInSemitones++;
                if (middlePitchInSemitones > maxPitchInSemitones)
                {
                    middlePitchInSemitones = minPitchInSemitones;
                }
                return;
            }

            if ((crotchetCount() % macro_numCrotchets) == 0)
            {
                // run macro event - a minim every time macro_numCrotchets occurs
                bool isNote1 = true;
                bool isTie1 = false;
                float timeMS = timeMS_per_minim();
                musicHistory += "Event to occur: MacroMinimNote" + " (isNote:" + isNote1.ToString() + ", isTie:" + isTie1.ToString() + ")" + Environment.NewLine;
                performEvent(timeMS, isNote1, isTie1, velocity);
                return;
            }

            bool isTie = (rnd.NextDouble() < chanceOfTie);

            float totalChance =
                    chanceOfSemibreveNote +
                    chanceOfMinimNote +
                    chanceOfCrotchetNote +
                    chanceOfMultipleItem_Quaver +
                    chanceOfMultipleItem_Triplet +
                    chanceOfMultipleItem_Semiquaver +
                    chanceOfSemibreveRest +
                    chanceOfMinimRest +
                    chanceOfCrotchetRest;
            float rnd1 = (float)rnd.NextDouble();
            if (checkProbabilityForTimingAndRunEvent(isTie, true, ref rnd1, chanceOfSemibreveNote, totalChance, timeMS_per_semibreve(), "SemibreveNote", velocity)) return;
            if (checkProbabilityForTimingAndRunEvent(isTie, true, ref rnd1, chanceOfMinimNote, totalChance, timeMS_per_minim(), "MinimNote", velocity)) return;
            if (checkProbabilityForTimingAndRunEvent(isTie, true, ref rnd1, chanceOfCrotchetNote, totalChance, timeMS_per_crotchet(), "CrotchetNote", velocity)) return;
            if (checkProbabilityForTimingAndRunMultipleEvent(2, chanceOfTieInMultipleItem_Quaver, chanceOfNoteInMultipleItem_Quaver, ref rnd1, chanceOfMultipleItem_Quaver, totalChance, timeMS_per_quaver(), "2-Quaver", velocity)) return;
            if (checkProbabilityForTimingAndRunMultipleEvent(3, chanceOfTieInMultipleItem_Triplet, chanceOfNoteInMultipleItem_Triplet, ref rnd1, chanceOfMultipleItem_Triplet, totalChance, timeMS_per_crotchet() / 3.0f, "3-Triplet", velocity)) return;
            if (checkProbabilityForTimingAndRunMultipleEvent(4, chanceOfTieInMultipleItem_Semiquaver, chanceOfNoteInMultipleItem_Semiquaver, ref rnd1, chanceOfMultipleItem_Semiquaver, totalChance, timeMS_per_semiquaver(), "4-Semiquaver", velocity)) return;
            if (checkProbabilityForTimingAndRunEvent(isTie, false, ref rnd1, chanceOfSemibreveRest, totalChance, timeMS_per_semibreve(), "SemibreveRest", velocity)) return;
            if (checkProbabilityForTimingAndRunEvent(isTie, false, ref rnd1, chanceOfMinimRest, totalChance, timeMS_per_minim(), "MinimRest", velocity)) return;
            if (checkProbabilityForTimingAndRunEvent(isTie, false, ref rnd1, chanceOfCrotchetRest, totalChance, timeMS_per_crotchet(), "CrotchetRest", velocity)) return;
        }

        private float speed_beatsPerSecond()
        {
            return speed_beatsPerMinute / 60.0f;
        }

        private float timeMS_per_semibreve()
        {
            // whole note
            return timeMS_per_crotchet() * 4.0f;
        }

        private float timeMS_per_minim()
        {
            // 1/2 note
            return timeMS_per_crotchet() * 2.0f;
        }

        private float timeMS_per_crotchet()
        {
            // 1/4 note
            return 1000.0f / speed_beatsPerSecond();
        }

        private float timeMS_per_quaver()
        {
            // 1/8 note
            return timeMS_per_crotchet() / 2.0f;
        }

        private float timeMS_per_semiquaver()
        {
            // 1/16 note
            return timeMS_per_crotchet() / 4.0f;
        }
    }
}