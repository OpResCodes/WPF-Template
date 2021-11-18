﻿using System;
using System.Threading;

namespace MatApp.Infrastructure.Dialogs
{
    public class ProgressHandling
    {
        public ProgressHandling()
        {
            ProgressReport = new Progress<double>();
            CancellationTokenSource = new CancellationTokenSource();
        }

        public CancellationTokenSource CancellationTokenSource { get; }

        public IProgress<double> ProgressReport { get; }

    }
}
