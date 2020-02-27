//-----------------------------------------------------------------------
// <copyright file="ARAnchorManagerExtensions.cs" company="Google">
//
// Copyright 2019 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Google.XR.ARCoreExtensions
{
    using System;
    using Google.XR.ARCoreExtensions.Internal;
    using UnityEngine;

    using UnityEngine.XR.ARFoundation;

    /// <summary>
    /// Extensions to AR Foundation's ARAnchorManager class.
    /// </summary>
    public static class ARAnchorManagerExtensions
    {
        private static readonly string k_GameObjectName = "ARCloudAnchor";

        /// <summary>
        /// Creates a new cloud anchor using an existing local Reference Point.
        /// <example>
        /// The sample code below illustrates how to host a cloud anchor.
        /// <code>
        /// private ARCloudAnchor m_CloudAnchor;
        ///
        /// void HostCloudReference(Pose pose)
        /// {
        ///     // Create a local Reference Point, you may also use another
        ///     // Reference Point you may already have.
        ///     ARAnchor localAnchor =
        ///         AnchorManager.AddAnchor(pose);
        ///
        ///     // Request the cloud anchor.
        ///     m_CloudAnchor =
        ///         AnchorManager.AddCloudAnchor(localAnchor);
        /// }
        ///
        /// void Update()
        /// {
        ///     if (m_CloudAnchor)
        ///     {
        ///         // Check the cloud anchor state.
        ///         CloudAnchorState cloudAnchorState =
        ///             m_CloudAnchor.cloudAnchorState;
        ///         if (cloudAnchorState == CloudAnchorState.Success)
        ///         {
        ///             myOtherGameObject.transform.SetParent
        ///                 m_CloudAnchor.transform, false);
        ///             m_CloudAnchor = null;
        ///         }
        ///         else if (cloudAnchorState == CloudAnchorState.TaskInProgress)
        ///         {
        ///             // Wait, not ready yet.
        ///         }
        ///         else
        ///         {
        ///             // An error has occurred.
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="anchorManager">The AnchorManager instance for extending.
        /// </param>
        /// <param name="anchor">The local <c>ARAnchor</c> to be used as the
        /// basis to host a new cloud anchor.</param>
        /// <returns>If successful, a <see cref="ARCloudAnchor"/>,
        /// otherwise <c>null</c>.</returns>
        public static ARCloudAnchor AddCloudAnchor(
            this ARAnchorManager anchorManager,
            ARAnchor anchor)
        {
            // Create the underlying ARCore Cloud Anchor.
            IntPtr cloudAnchorHandle = SessionApi.HostCloudAnchor(
                ARCoreExtensions.Instance.CurrentARCoreSessionHandle,
                anchor.AnchorHandle());
            if (cloudAnchorHandle == IntPtr.Zero)
            {
                return null;
            }

            // Create the GameObject that is the cloud anchor.
            ARCloudAnchor cloudAnchor =
                (new GameObject(k_GameObjectName)).AddComponent<ARCloudAnchor>();
            if (cloudAnchor)
            {
                cloudAnchor.SetAnchorHandle(cloudAnchorHandle);
            }

            // Parent the new cloud anchor to the session origin.
            cloudAnchor.transform.SetParent(
                ARCoreExtensions.Instance.SessionOrigin.trackablesParent, false);

            return cloudAnchor;
        }

        /// <summary>
        /// Creates a new local cloud anchor from the provided Id.
        /// <example>
        /// The sample code below illustrates how to resolve a cloud anchor.
        /// <code>
        /// private ARCloudAnchor m_CloudAnchor;
        ///
        /// void ResolveCloudReference(string cloudReferenceId)
        /// {
        ///     // Request the cloud anchor.
        ///     m_CloudAnchor =
        ///         AnchorManager.ResolveCloudReferenceId(cloudReferenceId);
        /// }
        ///
        /// void Update()
        /// {
        ///     if (m_CloudAnchor)
        ///     {
        ///         // Check the cloud anchor state.
        ///         CloudAnchorState cloudAnchorState =
        ///             m_CloudAnchor.cloudAnchorState;
        ///         if (cloudAnchorState == CloudAnchorState.Success)
        ///         {
        ///             myOtherGameObject.transform.SetParent
        ///                 m_CloudAnchor.transform, false);
        ///             m_CloudAnchor = null;
        ///         }
        ///         else if (cloudAnchorState == CloudAnchorState.TaskInProgress)
        ///         {
        ///             // Wait, not ready yet.
        ///         }
        ///         else
        ///         {
        ///             // An error has occurred.
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="anchorManager">The AnchorManager instance for extending.
        /// </param>
        /// <param name="cloudReferenceId">String representing the cloud reference.</param>
        /// <returns>If successful, a <see cref="ARCloudAnchor"/>,
        /// otherwise <c>null</c>.</returns>
        public static ARCloudAnchor ResolveCloudReferenceId(
            this ARAnchorManager anchorManager,
            string cloudReferenceId)
        {
            // Create the underlying ARCore Cloud Anchor.
            IntPtr cloudAnchorHandle = SessionApi.ResolveCloudAnchor(
                ARCoreExtensions.Instance.CurrentARCoreSessionHandle,
                cloudReferenceId);
            if (cloudAnchorHandle == IntPtr.Zero)
            {
                return null;
            }

            // Create the GameObject that is the cloud anchor.
            ARCloudAnchor cloudAnchor =
                (new GameObject(k_GameObjectName)).AddComponent<ARCloudAnchor>();
            if (cloudAnchor)
            {
                cloudAnchor.SetAnchorHandle(cloudAnchorHandle);
            }

            // Parent the new cloud anchor to the session origin.
            cloudAnchor.transform.SetParent(
                ARCoreExtensions.Instance.SessionOrigin.trackablesParent, false);

            return cloudAnchor;
        }
    }
}
