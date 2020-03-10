using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.TextFormatting;
using MS.Internal.PresentationCore;

namespace ysoserial_frmv2
{
    [Serializable]
    public sealed class TextFormattingRunProperties : TextRunProperties, ISerializable, IObjectReference
    {
         [NonSerialized]
        private Typeface _typeface;

        // Token: 0x040001EA RID: 490
        [NonSerialized]
        private double? _size;

        // Token: 0x040001EB RID: 491
        [NonSerialized]
        private double? _hintingSize;

        // Token: 0x040001EC RID: 492
        [NonSerialized]
        private double? _foregroundOpacity;

        // Token: 0x040001ED RID: 493
        [NonSerialized]
        private double? _backgroundOpacity;

        // Token: 0x040001EE RID: 494
        [NonSerialized]
        private Brush _foregroundBrush;

        // Token: 0x040001EF RID: 495
        [NonSerialized]
        private Brush _backgroundBrush;

        // Token: 0x040001F0 RID: 496
        [NonSerialized]
        private TextDecorationCollection _textDecorations;

        // Token: 0x040001F1 RID: 497
        [NonSerialized]
        private TextEffectCollection _textEffects;

        // Token: 0x040001F2 RID: 498
        [NonSerialized]
        private CultureInfo _cultureInfo;

        // Token: 0x040001F3 RID: 499
        [NonSerialized]
        private bool? _bold;

        // Token: 0x040001F4 RID: 500
        [NonSerialized]
        private bool? _italic;

        // Token: 0x040001F5 RID: 501
        [NonSerialized]
        private static List<TextFormattingRunProperties> ExistingProperties = new List<TextFormattingRunProperties>();

        // Token: 0x040001F6 RID: 502
        [NonSerialized]
        private static TextFormattingRunProperties EmptyProperties = new TextFormattingRunProperties();

        // Token: 0x040001F7 RID: 503
        [NonSerialized]
        private static TextEffectCollection EmptyTextEffectCollection = new TextEffectCollection();

        // Token: 0x040001F8 RID: 504
        [NonSerialized]
        private static TextDecorationCollection EmptyTextDecorationCollection = new TextDecorationCollection();

        private object GetObjectFromSerializationInfo(string name, SerializationInfo info)
        {
            string @string = info.GetString(name);
            if (@string == "null")
            {
                return null;
            }
            return XamlReader.Parse(@string);
        }
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("BackgroundBrush", this.BackgroundBrushEmpty ? "null" : XamlWriter.Save(this.BackgroundBrush));
            info.AddValue("ForegroundBrush", this.ForegroundBrushEmpty ? "null" : XamlWriter.Save(this.ForegroundBrush));
            info.AddValue("FontHintingSize", this.FontHintingEmSizeEmpty ? "null" : XamlWriter.Save(this.FontHintingEmSize));
            info.AddValue("FontRenderingSize", this.FontRenderingEmSizeEmpty ? "null" : XamlWriter.Save(this.FontRenderingEmSize));
            info.AddValue("TextDecorations", this.TextDecorationsEmpty ? "null" : XamlWriter.Save(this.TextDecorations));
            info.AddValue("TextEffects", this.TextEffectsEmpty ? "null" : XamlWriter.Save(this.TextEffects));
            info.AddValue("CultureInfoName", this.CultureInfoEmpty ? "null" : XamlWriter.Save(this.CultureInfo.Name));
            info.AddValue("FontFamily", this.TypefaceEmpty ? "null" : XamlWriter.Save(this.Typeface.FontFamily));
            info.AddValue("Italic", this.ItalicEmpty ? "null" : XamlWriter.Save(this.Italic));
            info.AddValue("Bold", this.BoldEmpty ? "null" : XamlWriter.Save(this.Bold));
            info.AddValue("ForegroundOpacity", this.ForegroundOpacityEmpty ? "null" : XamlWriter.Save(this.ForegroundOpacity));
            info.AddValue("BackgroundOpacity", this.BackgroundOpacityEmpty ? "null" : XamlWriter.Save(this.BackgroundOpacity));
            if (!this.TypefaceEmpty)
            {
                info.AddValue("Typeface.Style", XamlWriter.Save(this.Typeface.Style));
                info.AddValue("Typeface.Weight", XamlWriter.Save(this.Typeface.Weight));
                info.AddValue("Typeface.Stretch", XamlWriter.Save(this.Typeface.Stretch));
            }
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public object GetRealObject(StreamingContext context)
        {
            return TextFormattingRunProperties.FindOrCreateProperties(this);
        }

        private bool IsEqual(TextFormattingRunProperties other)
        {
            return this._size == other._size && this._hintingSize == other._hintingSize && TextFormattingRunProperties.TypefacesEqual(this._typeface, other._typeface) && this._cultureInfo == other._cultureInfo && this._textDecorations == other._textDecorations && this._textEffects == other._textEffects && this._italic == other._italic && this._bold == other._bold && this._foregroundOpacity == other._foregroundOpacity && this._backgroundOpacity == other._backgroundOpacity && TextFormattingRunProperties.BrushesEqual(this._foregroundBrush, other._foregroundBrush) && TextFormattingRunProperties.BrushesEqual(this._backgroundBrush, other._backgroundBrush);
        }
        internal static bool BrushesEqual(Brush brush, Brush other)
        {
            if (brush == null)
            {
                return other == null;
            }
            if (brush.Opacity == 0.0 && other.Opacity == 0.0)
            {
                return true;
            }
            SolidColorBrush solidColorBrush = brush as SolidColorBrush;
            SolidColorBrush solidColorBrush2 = other as SolidColorBrush;
            if (solidColorBrush != null && solidColorBrush2 != null)
            {
                return (solidColorBrush.Color.A == 0 && solidColorBrush2.Color.A == 0) || (solidColorBrush.Color == solidColorBrush2.Color && Math.Abs(solidColorBrush.Opacity - solidColorBrush2.Opacity) < 0.01);
            }
            return brush.Equals(other);
        }
        private static bool TypefacesEqual(Typeface typeface, Typeface other)
        {
            if (typeface == null)
            {
                return other == null;
            }
            return typeface.Equals(other);
        }

        internal static TextFormattingRunProperties FindOrCreateProperties(TextFormattingRunProperties properties)
        {
            TextFormattingRunProperties textFormattingRunProperties = TextFormattingRunProperties.ExistingProperties.Find((TextFormattingRunProperties other) => properties.IsEqual(other));
            if (textFormattingRunProperties == null)
            {
                properties.FreezeEverything();
                TextFormattingRunProperties.ExistingProperties.Add(properties);
                return properties;
            }
            return textFormattingRunProperties;
        }

        private void FreezeEverything()
        {
            this.FreezeForegroundBrush();
            this.FreezeBackgroundBrush();
            this.FreezeTextEffects();
            this.FreezeTextDecorations();
        }
        private void FreezeTextEffects()
        {
            if (this._textEffects != null && this._textEffects.CanFreeze)
            {
                this._textEffects.Freeze();
            }
        }
        private void FreezeTextDecorations()
        {
            if (this._textDecorations != null && this._textDecorations.CanFreeze)
            {
                this._textDecorations.Freeze();
            }
        }
        private void FreezeForegroundBrush()
        {
            if (this._foregroundBrush != null && this._foregroundBrush.CanFreeze)
            {
                this._foregroundBrush.Freeze();
            }
        }
        private void FreezeBackgroundBrush()
        {
            if (this._backgroundBrush != null && this._backgroundBrush.CanFreeze)
            {
                this._backgroundBrush.Freeze();
            }
        }

        public override Brush BackgroundBrush
        {
            get
            {
                return this._backgroundBrush ?? Brushes.Transparent;
            }
        }

        // Token: 0x1700027E RID: 638
        // (get) Token: 0x06000859 RID: 2137 RVA: 0x0001A6AA File Offset: 0x000188AA
        public override CultureInfo CultureInfo
        {
            get
            {
                return this._cultureInfo ?? CultureInfo.CurrentCulture;
            }
        }

        // Token: 0x1700027F RID: 639
        // (get) Token: 0x0600085A RID: 2138 RVA: 0x0001A6BC File Offset: 0x000188BC
        public override double FontHintingEmSize
        {
            get
            {
                double? hintingSize = this._hintingSize;
                if (hintingSize == null)
                {
                    return 0.0;
                }
                return hintingSize.GetValueOrDefault();
            }
        }

        // Token: 0x17000280 RID: 640
        // (get) Token: 0x0600085B RID: 2139 RVA: 0x0001A6EC File Offset: 0x000188EC
        public override double FontRenderingEmSize
        {
            get
            {
                double? size = this._size;
                if (size == null)
                {
                    return 0.0;
                }
                return size.GetValueOrDefault();
            }
        }

        // Token: 0x17000281 RID: 641
        // (get) Token: 0x0600085C RID: 2140 RVA: 0x0001A71A File Offset: 0x0001891A
        public override Brush ForegroundBrush
        {
            get
            {
                return this._foregroundBrush ?? Brushes.Transparent;
            }
        }

        // Token: 0x17000282 RID: 642
        // (get) Token: 0x0600085D RID: 2141 RVA: 0x0001A72B File Offset: 0x0001892B
        public bool Italic
        {
            get
            {
                return this._italic != null && this._italic.Value;
            }
        }

        // Token: 0x17000283 RID: 643
        // (get) Token: 0x0600085E RID: 2142 RVA: 0x0001A747 File Offset: 0x00018947
        public bool Bold
        {
            get
            {
                return this._bold != null && this._bold.Value;
            }
        }

        // Token: 0x17000284 RID: 644
        // (get) Token: 0x0600085F RID: 2143 RVA: 0x0001A763 File Offset: 0x00018963
        public double ForegroundOpacity
        {
            get
            {
                if (this._foregroundOpacity == null)
                {
                    return 1.0;
                }
                return this._foregroundOpacity.Value;
            }
        }

        // Token: 0x17000285 RID: 645
        // (get) Token: 0x06000860 RID: 2144 RVA: 0x0001A787 File Offset: 0x00018987
        public double BackgroundOpacity
        {
            get
            {
                if (this._backgroundOpacity == null)
                {
                    return 1.0;
                }
                return this._backgroundOpacity.Value;
            }
        }

        // Token: 0x17000286 RID: 646
        // (get) Token: 0x06000861 RID: 2145 RVA: 0x0001A7AB File Offset: 0x000189AB
        public override TextDecorationCollection TextDecorations
        {
            get
            {
                return this._textDecorations ?? TextFormattingRunProperties.EmptyTextDecorationCollection;
            }
        }

        // Token: 0x17000287 RID: 647
        // (get) Token: 0x06000862 RID: 2146 RVA: 0x0001A7BC File Offset: 0x000189BC
        public override TextEffectCollection TextEffects
        {
            get
            {
                return this._textEffects ?? TextFormattingRunProperties.EmptyTextEffectCollection;
            }
        }

        // Token: 0x17000288 RID: 648
        // (get) Token: 0x06000863 RID: 2147 RVA: 0x0001A7CD File Offset: 0x000189CD
        public override Typeface Typeface
        {
            get
            {
                return this._typeface;
            }
        }

        // Token: 0x17000289 RID: 649
        // (get) Token: 0x06000864 RID: 2148 RVA: 0x0001A7D5 File Offset: 0x000189D5
        public bool BackgroundBrushEmpty
        {
            get
            {
                return this._backgroundBrush == null;
            }
        }

        // Token: 0x1700028A RID: 650
        // (get) Token: 0x06000865 RID: 2149 RVA: 0x0001A7E0 File Offset: 0x000189E0
        public bool BackgroundOpacityEmpty
        {
            get
            {
                return this._backgroundOpacity == null;
            }
        }

        // Token: 0x1700028B RID: 651
        // (get) Token: 0x06000866 RID: 2150 RVA: 0x0001A7F0 File Offset: 0x000189F0
        public bool ForegroundOpacityEmpty
        {
            get
            {
                return this._foregroundOpacity == null;
            }
        }

        // Token: 0x1700028C RID: 652
        // (get) Token: 0x06000867 RID: 2151 RVA: 0x0001A800 File Offset: 0x00018A00
        public bool BoldEmpty
        {
            get
            {
                return this._bold == null;
            }
        }

        // Token: 0x1700028D RID: 653
        // (get) Token: 0x06000868 RID: 2152 RVA: 0x0001A810 File Offset: 0x00018A10
        public bool ItalicEmpty
        {
            get
            {
                return this._italic == null;
            }
        }

        // Token: 0x1700028E RID: 654
        // (get) Token: 0x06000869 RID: 2153 RVA: 0x0001A820 File Offset: 0x00018A20
        public bool CultureInfoEmpty
        {
            get
            {
                return this._cultureInfo == null;
            }
        }

        // Token: 0x1700028F RID: 655
        // (get) Token: 0x0600086A RID: 2154 RVA: 0x0001A82B File Offset: 0x00018A2B
        public bool FontHintingEmSizeEmpty
        {
            get
            {
                return this._hintingSize == null;
            }
        }

        // Token: 0x17000290 RID: 656
        // (get) Token: 0x0600086B RID: 2155 RVA: 0x0001A83B File Offset: 0x00018A3B
        public bool FontRenderingEmSizeEmpty
        {
            get
            {
                return this._size == null;
            }
        }

        // Token: 0x17000291 RID: 657
        // (get) Token: 0x0600086C RID: 2156 RVA: 0x0001A84B File Offset: 0x00018A4B
        public bool ForegroundBrushEmpty
        {
            get
            {
                return this._foregroundBrush == null;
            }
        }

        // Token: 0x17000292 RID: 658
        // (get) Token: 0x0600086D RID: 2157 RVA: 0x0001A856 File Offset: 0x00018A56
        public bool TextDecorationsEmpty
        {
            get
            {
                return this._textDecorations == null;
            }
        }

        // Token: 0x17000293 RID: 659
        // (get) Token: 0x0600086E RID: 2158 RVA: 0x0001A861 File Offset: 0x00018A61
        public bool TextEffectsEmpty
        {
            get
            {
                return this._textEffects == null;
            }
        }

        // Token: 0x17000294 RID: 660
        // (get) Token: 0x0600086F RID: 2159 RVA: 0x0001A86C File Offset: 0x00018A6C
        public bool TypefaceEmpty
        {
            get
            {
                return this._typeface == null;
            }
        }
    }
}
