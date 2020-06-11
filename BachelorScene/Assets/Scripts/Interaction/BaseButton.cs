using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseButton : MonoBehaviour
{
    public Material m_activeMaterial;
    public Material m_inactiveMaterial;
    public Material m_highlightMaterial;

    public MeshRenderer m_meshRenderer;

    public ButtonInteracter.ButtonChoice m_button = ButtonInteracter.ButtonChoice.HandTrigger;

    public bool m_active;

    public bool m_activateOnly = false;
    public bool m_deactivateOnly = false;

    protected bool highlighted = false;
    protected bool waitForInactive = false;

    public delegate void ButtonInteraction(object button, object from);
    public delegate void ButtonHoverInteraction(object button, object from, bool active);

    public event ButtonInteraction OnActivatedEvent;
    public event ButtonInteraction OnDectivatedEvent;
    public event ButtonInteraction OnHighlightedEvent;
    public event ButtonInteraction OnUnhighlightedEvent;
    public event ButtonInteraction OnUnInteractEvent;
    public event ButtonHoverInteraction OnInteractEvent;

    void Start()
    {
        if (m_meshRenderer != null)
        {
            m_meshRenderer.material = m_active ? m_activeMaterial : m_inactiveMaterial;
        }
    }

    void Update()
    {
        UpdateMaterial();
    }

    // If the button can be activated
    public bool CanActivate()
    {
        return !m_deactivateOnly && !m_active;
    }

    // If the button can be deactivated
    public bool CanDeactivate()
    {
        return !m_activateOnly && m_active;
    }

    // Every interaction within the button collider
    public virtual void OnInteract(object from, bool activating)
    {
        if (!waitForInactive)
        {
            if (CanActivate())
            {
                if (activating)
                {
                    OnActivate(from);
                }
                else if (!highlighted)
                {
                    OnHighlight(from);
                }
            }

            else if (CanDeactivate())
            {
                if (activating)
                {
                    OnDeactivate(from);
                }
                else if (!highlighted)
                {
                    OnHighlight(from);
                }
            }
        }

        if (waitForInactive && !activating)
        {
            waitForInactive = false;
        }

        OnInteractEvent?.Invoke(this, from, activating);
    }


    // Update button material
    public void UpdateMaterial()
    {
        if (m_meshRenderer != null)
        {
            Material newMaterial = m_active ? m_activeMaterial : m_inactiveMaterial;

            if (highlighted)
            {
                newMaterial = m_highlightMaterial;
            }

            if (newMaterial != m_meshRenderer.material)
            {
                m_meshRenderer.material = newMaterial;
            }
        }
    }


    // On interacter entering collider
    public virtual void OnHighlight(object from = null)
    {
        OnHighlightedEvent?.Invoke(this, from);

        highlighted = true;
    }

    // On interacter leaving collider
    public virtual void OnUnhighlighted(object from = null)
    {
        OnUnhighlightedEvent?.Invoke(this, from);

        highlighted = false;
    }


    // Interacter activating the button
    public virtual void OnActivate(object from = null)
    {
        OnActivatedEvent?.Invoke(this, from);

        m_active = true;
        highlighted = false;
        waitForInactive = true;
    }

    // Interacter deactivating the button
    public virtual void OnDeactivate(object from = null)
    {
        OnDectivatedEvent?.Invoke(this, from);

        m_active = false;
        highlighted = false;
        waitForInactive = true;
    }

    // Interacter leaving button collider
    public virtual void OnUninteract(object from = null)
    {
        if (highlighted)
        {
            OnUnhighlighted(from);
        }

        OnUnInteractEvent?.Invoke(this, from);
    }

}
