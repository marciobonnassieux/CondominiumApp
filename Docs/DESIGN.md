# Azure Concierge Design System

### 1. Overview & Creative North Star
**Creative North Star: "The Digital Sentinel"**
Azure Concierge is a high-end editorial framework designed for security and residential management. It rejects the "utility-first" clutter of traditional industrial software in favor of a sophisticated, calm, and authoritative interface. The system uses a deep midnight blue palette juxtaposed with soft, airy backgrounds to create a sense of trust and clarity. By utilizing asymmetric container shapes and varying tonal depths, we create a rhythmic flow that guides the eye naturally toward critical actions without overwhelming the user.

### 2. Colors
The palette is rooted in **#002045 (Deep Midnight)** and **#f8f9ff (Cool Azure)**, creating a professional and focused environment.

*   **The "No-Line" Rule:** Direct sectioning with 1px solid lines is strictly prohibited. Hierarchy and separation must be achieved through background shifts (e.g., transitioning from `surface` to `surface_container_low`).
*   **Surface Hierarchy & Nesting:** Use `surface_container_lowest` (#ffffff) for the highest contrast interactive cards. Lower tiers like `surface_container` are used for informational groupings.
*   **The "Glass & Gradient" Rule:** Navigation and floating panels should utilize a `backdrop-blur` of 20px with an 80% opacity white fill to simulate premium glass.
*   **Signature Textures:** A subtle radial mesh gradient is applied to the main background, moving from a soft blue (`rgba(214, 227, 255, 0.3)`) at the top-left to a warm, barely-perceptible gold (`rgba(233, 193, 118, 0.05)`) at the top-center.

### 3. Typography
Azure Concierge utilizes a dual-font strategy: **Manrope** for high-impact headlines and **Inter** for utilitarian data and body text.

*   **Headline Scale:** Uses Manrope with extra-bold weights and tight letter-spacing (-0.025em).
    *   **Display/H1:** 36px (2.25rem) — Used for primary dashboard titles.
    *   **Large Headline:** 30px (1.875rem) — For major section headers.
    *   **Standard Headline:** 18px (1.125rem) — For card titles.
*   **Body & Label Scale:** Uses Inter for maximum legibility at small sizes.
    *   **Body Standard:** 14px (0.875rem) — General information.
    *   **Micro Labels:** 10px - 11px (0.625rem - 0.7rem) — Used for "O App do Condomínio" and tracking status, always in uppercase with wide tracking (0.1em).

### 4. Elevation & Depth
Depth is communicated through tonal layering and light-refractive properties rather than heavy dropshadows.

*   **The Layering Principle:** A card sitting on the `surface` should be `surface_container_lowest` (#ffffff). A secondary info card should use `surface_container` to appear "recessed."
*   **Ambient Shadows:** We utilize the **Shadow-LG** preset for primary action buttons: a soft, wide-dispersion shadow (`0 10px 15px -3px rgba(0, 0, 0, 0.1)`).
*   **Glassmorphism:** The Bottom Navigation utilizes a `backdrop-blur-xl` with a top-oriented shadow (`0 -16px 48px rgba(13, 28, 47, 0.06)`) to feel as if it is floating above the content.

### 5. Components
*   **Hero Buttons:** Large primary buttons (e.g., "RECEBER ENCOMENDAS") use the `primary` color (#002045) with `on-primary` text and a subtle 200ms scale-down (95%) on interaction.
*   **Asymmetric Stat Cards:** Use a 16px (1rem) border radius. Include a background "ghost" icon (e.g., a circle at 5% opacity) to add visual interest without cluttering the data.
*   **Action Chips:** Square-ish icons with 12px (0.75rem) radius, utilizing `surface-container-lowest` for high contrast against the mesh background.
*   **Activity Feed:** Items are grouped in semi-transparent white containers (50% opacity) to let the background mesh peak through, creating an editorial "scrapbook" feel.

### 6. Do's and Don'ts
*   **Do:** Use uppercase for labels and action triggers to establish a clear "instructional" tone.
*   **Do:** Use 24px (1.5rem) spacing as the standard vertical rhythm between sections.
*   **Don't:** Use pure black (#000000) for text. Use `on-surface` (#0d1c2f) to maintain the blue-toned sophistication.
*   **Don't:** Overuse the gold/tertiary color. It is reserved for high-value status icons and active navigation states only.
*   **Do:** Ensure all interactive elements have a minimum touch target of 44px, even if the visual container is smaller.