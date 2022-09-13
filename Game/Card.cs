using System;
using System.Windows.Forms;

namespace Game
{
        internal struct Card
        {
            private byte m_ImageIndex;
            private bool m_Flipped;

            public Card(byte i_Value, bool i_Flipped)
            {
                m_ImageIndex = i_Value;
                m_Flipped = i_Flipped;
            }

            public Card(byte i_Value)
                : this()
            {
                m_ImageIndex = i_Value;
                m_Flipped = false;
            }

            // ===================================================================
            // Properties
            // ===================================================================
            public byte ImageIndex
            {
                get
                {
                    byte retunValue = byte.MaxValue;
                    if (Flipped)
                    {
                        retunValue = m_ImageIndex;
                    }

                    return retunValue;
                }

                set
                {
                    m_ImageIndex = value;
                }
            }

            public bool Flipped
            {
                get
                {
                    return m_Flipped;
                }

                set
                {
                    m_Flipped = value;
                }
            }

            public static bool operator ==(Card i_Card1, Card i_Card2)
            {
                return i_Card1.Equals(i_Card2);
            }

            public static bool operator !=(Card i_Card1, Card i_Card2)
            {
                return !i_Card1.Equals(i_Card2);
            }

            public override bool Equals(object i_ComperTo)
            {
                return this.ImageIndex == ((Card)i_ComperTo).ImageIndex;
            }

            public override int GetHashCode()
            {
                int hashCode = 1148891178;
                hashCode = (hashCode * -1521134295) + m_ImageIndex.GetHashCode();
                hashCode = (hashCode * -1521134295) + m_Flipped.GetHashCode();
                hashCode = (hashCode * -1521134295) + ImageIndex.GetHashCode();
                hashCode = (hashCode * -1521134295) + Flipped.GetHashCode();
                return hashCode;
            }

            internal void Card_Clicked(object i_Sender, EventArgs i_EventArgs)
            {
                if (i_Sender is Button clickedButton)
                {
                    clickedButton.Enabled = false;
                    Flipped = true;
                }
            }
        }
}
