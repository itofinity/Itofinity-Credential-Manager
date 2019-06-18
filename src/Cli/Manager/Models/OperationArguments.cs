using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Itofinity.Refit.Cli.Utils;

using Microsoft.Extensions.CommandLineUtils;

namespace Manager.Model 
{
    public class OperationArguments 
    {
        private static ILogger Logger { get; } = ApplicationLogging.CreateLogger<OperationArguments>();

        private Dictionary<string, List<string>> _options = new Dictionary<string, List<string>>();

        public Dictionary<string, List<string>> Options => _options;

        /// <summary>
        /// Reads git-credential formatted input from `<paramref name="readableStream"/>`, parses the data, and populates `<seealso cref="TargetUri"/>`.
        /// </summary>
        /// <param name="readableStream">
        /// Readable stream with credential protocol formatted information.
        /// <para/>
        /// Please see 'https://www.kernel.org/pub/software/scm/git/docs/git-credential.html' for information about credential protocol format.
        /// </param>
        /// <exception cref="ArgumentNullException">When `<paramref name="readableStream"/>` is `<see langword="null"/>`.</exception>
        /// <exception cref="ArgumentException">When `<paramref name="readableStream"/>` is not readable.</exception>
        /// <exception cref="InvalidOperationException">When data read from `<paramref name="readableStream"/>` is in an unexpected format.</exception>
        public virtual async Task ReadInput(Stream readableStream)
        {
            if (readableStream is null)
                throw new ArgumentNullException(nameof(readableStream));

            if (readableStream == Stream.Null || !readableStream.CanRead)
            {
                var inner = new InvalidDataException("Stream must be readable.");
                throw new ArgumentException(inner.Message, nameof(readableStream), inner);
            }

            byte[] buffer = new byte[4096];
            int read = 0;

            int r;
            while ((r = await readableStream.ReadAsync(buffer, read, buffer.Length - read)) > 0)
            {
                read += r;

                // If we've filled the buffer, make it larger this could hit an out of memory
                // condition, but that'd require the called to be attempting to do so, since
                // that's not a security threat we can safely ignore that and allow NetFx to
                // handle it.
                if (read == buffer.Length)
                {
                    Array.Resize(ref buffer, buffer.Length * 2);
                }

                if ((read > 0 && read < 3 && buffer[read - 1] == '\n'))
                {
                    var inner = new InvalidDataException("Invalid input, please see 'https://www.kernel.org/pub/software/scm/git/docs/git-credential.html'.");
                    throw new InvalidOperationException(inner.Message, inner);
                }

                // The input ends with LFLF, check for that and break the read loop unless
                // input is coming from CLRF system, in which case it'll be CLRFCLRF.
                if ((buffer[read - 2] == '\n'
                        && buffer[read - 1] == '\n')
                    || (buffer[read - 4] == '\r'
                        && buffer[read - 3] == '\n'
                        && buffer[read - 2] == '\r'
                        && buffer[read - 1] == '\n'))
                    break;
            }

            // Git uses UTF-8 for string, don't let the OS decide how to decode it instead
            // we'll actively decode the UTF-8 block ourselves.
            string input = Encoding.UTF8.GetString(buffer, 0, read);

            string username = null;
            string password = null;

            // The `StringReader` is just useful.
            using (var reader = new StringReader(input))
            {
                string line;
                while (!string.IsNullOrWhiteSpace((line = reader.ReadLine())))
                {
                    Logger.LogDebug($"line={line}");
                    string[] pair = line.Split(new[] { '=' }, 2);

                    if (pair.Length == 2)
                    {
                        var values = new List<string>();
                        values.AddRange(pair[1].Split(","));
                        _options[pair[0]] = values;

                        // This is a GCM only addition to the Git-Credential specification. The intent is to
                        // facilitate debugging without the need for running git.exe to debug git-remote-http(s)
                        // command line values.
                        //else if ("_url".Equals(pair[0], StringComparison.Ordinal))
                        //{
                        //    _gitRemoteHttpCommandLine = pair[1];
                        //}
                    }
                }
            }

            // Cache the username and password provided by the caller (Git).
            /* 
            if (username != null)
            {
                _username = username;

                // Only bother checking the password if there was a username credentials
                // cannot be only a password (though passwords are optional).
                if (password != null)
                {
                    _credentials = new Credential(username, password);
                }
            }

            CreateTargetUri();
            */
        }
    }
}