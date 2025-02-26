﻿// <copyright file="UITabstrips.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace AlgernonCommons.UI
{
    using ColossalFramework.UI;
    using UnityEngine;

    /// <summary>
    /// UI tabstrips.
    /// </summary>
    public static class UITabstrips
    {
        /// <summary>
        /// Adds a tabstrip panel.
        /// </summary>
        /// <param name="parent">Parent component.</param>
        /// <param name="posX">Relative X postion.</param>
        /// <param name="posY">Relative Y position.</param>
        /// <param name="width">Tabstrip panel width.</param>
        /// <param name="height">Tabstrip panel height (including tabs).</param>
        /// <param name="container">Tabstrip tab container instance.</param>
        /// <returns>New tabstrip.</returns>
        public static UITabstrip AddTabStrip(UIComponent parent, float posX, float posY, float width, float height, out UITabContainer container)
        {
            // Basic setup.
            UITabstrip tabStrip = parent.AddUIComponent<UITabstrip>();
            tabStrip.relativePosition = new Vector2(posX, posY);
            tabStrip.width = width;
            tabStrip.height = height;
            tabStrip.clipChildren = false;

            // Tab container (the panels underneath each tab).
            UITabContainer tabContainer = parent.AddUIComponent<UITabContainer>();
            tabContainer.name = "TabContainer";
            tabContainer.relativePosition = new Vector2(posX, posY + 30f);
            tabContainer.width = width;
            tabContainer.height = height;
            tabContainer.clipChildren = false;
            tabStrip.tabPages = tabContainer;

            container = tabContainer;
            return tabStrip;
        }

        /// <summary>
        /// Adds a text-based tab to a UI tabstrip.
        /// </summary>
        /// <param name="tabStrip">UIT tabstrip to add to.</param>
        /// <param name="tabName">Name of this tab.</param>
        /// <param name="tabIndex">Index number of this tab.</param>
        /// <param name="button">Tab button instance.</param>
        /// <param name="width">Tab width.</param>
        /// <param name="autoLayout">Default autoLayout setting.</param>
        /// <returns>UIPanel instance for the new tab panel.</returns>
        public static UIPanel AddTextTab(UITabstrip tabStrip, string tabName, int tabIndex, out UIButton button, float width = 170f, bool autoLayout = false)
        {
            // Create tab.
            UIButton tabButton = tabStrip.AddTab(tabName);

            // Sprites.
            tabButton.normalBgSprite = "SubBarButtonBase";
            tabButton.disabledBgSprite = "SubBarButtonBaseDisabled";
            tabButton.focusedBgSprite = "SubBarButtonBaseFocused";
            tabButton.hoveredBgSprite = "SubBarButtonBaseHovered";
            tabButton.pressedBgSprite = "SubBarButtonBasePressed";

            // Tooltip.
            tabButton.tooltip = tabName;

            // Force width.
            tabButton.width = width;

            // Get tab root panel.
            UIPanel rootPanel = tabStrip.tabContainer.components[tabIndex] as UIPanel;

            // Panel setup.
            rootPanel.autoLayout = autoLayout;
            rootPanel.autoLayoutDirection = LayoutDirection.Vertical;
            rootPanel.autoLayoutPadding.top = 5;
            rootPanel.autoLayoutPadding.left = 10;

            button = tabButton;

            return rootPanel;
        }

        /// <summary>
        /// Adds an icon-based tab to a UI tabstrip.
        /// </summary>
        /// <param name="tabStrip">UIT tabstrip to add to.</param>
        /// <param name="tabName">Name of this tab.</param>
        /// <param name="tabIndex">Index number of this tab.</param>
        /// <param name="iconNames">Icon sprite names.</param>
        /// <param name="atlasNames">Icon atlas names.</param>
        /// <param name="width">Tab width.</param>
        /// <param name="autoLayout">Default autoLayout setting.</param>
        /// <returns>UIPanel instance for the new tab panel.</returns>
        public static UIPanel AddIconTab(UITabstrip tabStrip, string tabName, int tabIndex, string[] iconNames, string[] atlasNames, float width = 120f, bool autoLayout = false)
        {
            // Layout constants.
            const float TabIconSize = 23f;

            // Create tab.
            UIPanel rootPanel = AddTextTab(tabStrip, tabName, tabIndex, out UIButton button, width, autoLayout);

            // Clear button text.
            button.text = string.Empty;

            // Add tab sprites.
            float spriteBase = (width - 2f) / iconNames.Length;
            float spriteOffset = (spriteBase - TabIconSize) / 2f;
            for (int i = 0; i < iconNames.Length; ++i)
            {
                UISprite thumbSprite = button.AddUIComponent<UISprite>();
                thumbSprite.relativePosition = new Vector2(1f + (spriteBase * i) + spriteOffset, 1f);
                thumbSprite.width = TabIconSize;
                thumbSprite.height = TabIconSize;
                thumbSprite.atlas = UITextures.GetTextureAtlas(atlasNames[i]);
                thumbSprite.spriteName = iconNames[i];

                // Put later sprites behind earlier sprites, for clarity.
                thumbSprite.SendToBack();
            }

            return rootPanel;
        }
    }
}