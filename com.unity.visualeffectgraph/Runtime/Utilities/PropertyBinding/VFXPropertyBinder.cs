using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityEngine.VFX.Utility
{
    /// <summary>
    /// A Behaviour that controls binding between Visual Effect Properties, and other scene values, through the use of VFXBinderBase
    /// </summary>
    [RequireComponent(typeof(VisualEffect))]
    [DefaultExecutionOrder(1)]
    [ExecuteInEditMode]
    public class VFXPropertyBinder : MonoBehaviour
    {
        /// <summary>
        /// Whether the bindings should be executed in editor (as preview)
        /// </summary>
        [SerializeField]
        protected bool m_ExecuteInEditor = true;

        /// <summary>
        /// The list of all Bindings attached to the binder, these bindings are managed by the VFXPropertyBinder and should be managed using the AddPropertyBinder, ClearPropertyBinders, RemovePropertyBinder, RemovePropertyBinders, and GetPropertyBinders.
        /// </summary>
        public List<VFXBinderBase> m_Bindings = new List<VFXBinderBase>();

        /// <summary>
        /// The Visual Effect component attached to the VFXPropertyBinder
        /// </summary>
        [SerializeField]
        protected VisualEffect m_VisualEffect;

        private void OnEnable()
        {
            m_VisualEffect = GetComponent<VisualEffect>();
        }

        private void Reset()
        {
            m_VisualEffect = GetComponent<VisualEffect>();
            m_Bindings = new List<VFXBinderBase>();
            m_Bindings.AddRange(gameObject.GetComponents<VFXBinderBase>());
            ClearPropertyBinders();
        }

        void Update()
        {
            if (!m_ExecuteInEditor && Application.isEditor && !Application.isPlaying) return;

            for (int i = 0; i < m_Bindings.Count; i++)
            {
                var binding = m_Bindings[i];

                if (binding == null)
                {
                    Debug.LogWarning(string.Format("Parameter binder at index {0} of GameObject {1} is null or missing", i, gameObject.name));
                    continue;
                }
                else
                {
                    if (binding.IsValid(m_VisualEffect))
                        binding.UpdateBinding(m_VisualEffect);
                }
            }
        }

        /// <summary>
        /// Adds a new PropertyBinder
        /// </summary>
        /// <typeparam name="T">the Type of Property Binder</typeparam>
        /// <returns>The PropertyBinder newly Created</returns>
        public T AddPropertyBinder<T>() where T : VFXBinderBase
        {
            return gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Adds a new PropertyBinder
        /// </summary>
        /// <typeparam name="T">the Type of Property Binder</typeparam>
        /// <returns>The PropertyBinder newly Created</returns>
        [Obsolete("Use AddPropertyBinder<T>() instead")]
        public T AddParameterBinder<T>() where T : VFXBinderBase
        {
            return AddPropertyBinder<T>();
        }

        /// <summary>
        /// Clears all the Property Binders
        /// </summary>
        public void ClearPropertyBinders()
        {
            var allBinders = GetComponents<VFXBinderBase>();
            foreach (var binder in allBinders) Destroy(binder);
        }

        /// <summary>
        /// Clears all the Property Binders
        /// </summary>
        [Obsolete("Please use ClearPropertyBinders() instead")]
        public void ClearParameterBinders()
        {
            ClearPropertyBinders();
        }

        /// <summary>
        /// Removes specified Property Binder
        /// </summary>
        /// <param name="binder">The VFXBinderBase to remove</param>
        public void RemovePropertyBinder(VFXBinderBase binder)
        {
            if (binder.gameObject == this.gameObject) Destroy(binder);
        }

        /// <summary>
        /// Removes specified Property Binder
        /// </summary>
        /// <param name="binder">The VFXBinderBase to remove</param>
        [Obsolete("Please use RemovePropertyBinder() instead")]
        public void RemoveParameterBinder(VFXBinderBase binder)
        {
            RemovePropertyBinder(binder);
        }

        /// <summary>
        /// Remove all Property Binders of Given Type
        /// </summary>
        /// <typeparam name="T">Specified VFXBinderBase type</typeparam>
        public void RemovePropertyBinders<T>() where T : VFXBinderBase
        {
            var allBinders = GetComponents<VFXBinderBase>();
            foreach (var binder in allBinders)
                if (binder is T) Destroy(binder);
        }

        /// <summary>
        /// Remove all Property Binders of Given Type
        /// </summary>
        /// <typeparam name="T">Specified VFXBinderBase type</typeparam>
        [Obsolete("Please use RemovePropertyBinders<T>() instead")]
        public void RemoveParameterBinders<T>() where T : VFXBinderBase
        {
            RemovePropertyBinders<T>();
        }

        /// <summary>
        /// Gets all VFXBinderBase of Given Type, attached to this VFXPropertyBinder
        /// </summary>
        /// <typeparam name="T">Specific VFXBinderBase type</typeparam>
        /// <returns>An IEnumerable of all VFXBinderBase</returns>
        public IEnumerable<T> GetPropertyBinders<T>() where T : VFXBinderBase
        {
            foreach (var binding in m_Bindings)
            {
                if (binding is T) yield return binding as T;
            }
        }

        /// <summary>
        /// Gets all VFXBinderBase of Given Type, attached to this VFXPropertyBinder
        /// </summary>
        /// <typeparam name="T">Specific VFXBinderBase type</typeparam>
        /// <returns>An IEnumerable of all VFXBinderBase</returns>
        [Obsolete("Please use GetPropertyBinders<T>() instead")]
        public IEnumerable<T> GetParameterBinders<T>() where T : VFXBinderBase
        {
            return GetPropertyBinders<T>();
        }
    }
}
