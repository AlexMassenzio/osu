// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Timing;

namespace osu.Game.Graphics.UserInterface
{
    /// <summary>
    /// Adds hover sounds to a drawable.
    /// Does not draw anything.
    /// </summary>
    public class HoverSounds : CompositeDrawable
    {
        private SampleChannel sampleHover;

        protected readonly HoverSampleSet SampleSet;

        private IAdjustableClock debounceClock;
        private readonly double debounceRate;

        public HoverSounds(HoverSampleSet sampleSet = HoverSampleSet.Normal)
        {
            SampleSet = sampleSet;
            RelativeSizeAxes = Axes.Both;
            debounceClock = new StopwatchClock();
            debounceClock.Start();
            debounceRate = 100d;
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (debounceClock.CurrentTime > debounceRate)
            {
                debounceClock.Seek(0d);
                sampleHover?.Play();
            }
            return base.OnHover(e);
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            sampleHover = audio.Sample.Get($@"UI/generic-hover{SampleSet.GetDescription()}");
        }
    }

    public enum HoverSampleSet
    {
        [Description("")]
        Loud,

        [Description("-soft")]
        Normal,

        [Description("-softer")]
        Soft
    }
}
