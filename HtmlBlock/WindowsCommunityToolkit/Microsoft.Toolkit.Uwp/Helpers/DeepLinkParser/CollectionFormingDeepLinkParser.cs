// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Windows.ApplicationModel.Activation;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// A version of <see cref="DeepLinkParser" /> which generates a comma-separated-list as the value for any option that is repeated in the query string
    /// </summary>
    /// <example>
    /// in OnLaunched of App.xaml.cs:
    /// <code lang="c#">
    /// if (e.PrelaunchActivated == false)
    /// {
    ///     if (rootFrame.Content == null)
    ///     {
    ///         var parser = CollectionFormingDeepLinkParser.Create(args);
    ///         if (parser["username"] == "John Doe")
    ///         {
    ///             // do work here
    ///         }
    ///         if (parser.Root == "Signup")
    ///         {
    ///             var preferences = parser["pref"].Split(',');    // now a string[] of all 'pref' querystring values passed in URI
    ///             rootFrame.Navigate(typeof(Signup));
    ///         }
    /// </code>
    /// </example>
    public class CollectionFormingDeepLinkParser : DeepLinkParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionFormingDeepLinkParser"/> class.
        /// </summary>
        protected CollectionFormingDeepLinkParser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionFormingDeepLinkParser" /> class.
        /// </summary>
        /// <param name="args">The <see cref="IActivatedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.ArgumentException">'args' is not a LaunchActivatedEventArgs instance</exception>
        protected CollectionFormingDeepLinkParser(IActivatedEventArgs args)
            : base(args)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionFormingDeepLinkParser" /> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="uri"/> is null</exception>
        protected CollectionFormingDeepLinkParser(Uri uri)
            : this(uri?.OriginalString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionFormingDeepLinkParser" /> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="uri"/> is null, empty, or consists only of whitespace characters</exception>
        protected CollectionFormingDeepLinkParser(string uri)
            : base(uri)
        {
        }

        /// <inheritdoc/>
        protected override void ParseUriString(string uri)
        {
            Uri validatedUri = ValidateSourceUri(uri);

            SetRoot(validatedUri);
            QueryParameterCollection queryParams = new Helpers.QueryParameterCollection(validatedUri);
            System.Collections.Generic.IEnumerable<IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>>> grouped = queryParams.GroupBy(pair => pair.Key);
            foreach (IGrouping<string, System.Collections.Generic.KeyValuePair<string, string>> group in grouped)
            { // adds the group to the base with ',' separating each item within a group
                Add(group.Key, string.Join(",", group.Select(item => item.Value)));
            }
        }
    }
}