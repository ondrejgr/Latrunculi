using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Obecná hrací deska
    /// </summary>
    public abstract class Board
    {
        private Pieces[,] _data = null;
        /// <summary>
        /// interní reprezentace desky
        /// </summary>
        private Pieces[,] Data
        {
            get
            {
                return _data;
            }
        }

        /// <summary>
        /// Přístup k hodnotám polí desky.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public Pieces this[Coord coord]
        {
            get
            {
                if (!IsInitiaized || (Data == null))
                    throw new InvalidOperationException("Pole hrací desky nelze přečíst, protože deska ještě nebyla inicializována.");

                if (!IsCoordValid(coord))
                    throw new IndexOutOfRangeException("Zadaná souřadnice je mimo rozsah hrací desky !");

                return Data[coord.x - 'A', coord.y - 1];
            }
            set
            {
                if (!IsInitiaized || (Data == null))
                    throw new InvalidOperationException("Pole hrací desky nelze nastavit, protože deska ještě nebyla inicializována.");

                if (!IsCoordValid(coord))
                    throw new IndexOutOfRangeException("Zadaná souřadnice je mimo rozsah hrací desky !");

                Data[coord.x - 'A', coord.y - 1] = value;
            }
        }

        /// <summary>
        /// zjisti pocet kamenu dane barvy
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public int GetNumberOfPieces(GameColorsEnum color)
        {
            Coord c = new Coord();
            int result = 0;

            for (char x = 'A'; x <= MaxX; x++)
            {
                for (byte y = 1; y <= MaxY; y++)
                {
                    c.Set(x, y);

                    switch(color)
                    {
                        case GameColorsEnum.plrBlack:
                            if (this[c] == Pieces.pcBlack || this[c] == Pieces.pcBlackKing)
                                result++;
                            break;
                        case GameColorsEnum.plrWhite:
                            if (this[c] == Pieces.pcWhite || this[c] == Pieces.pcWhiteKing)
                                result++;
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Přetížit pro nastavení max. hodnoty první souřadnice
        /// </summary>
        public virtual char MaxX
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Přetížit pro nastavení max. hodnoty druhé souřadnice
        /// </summary>
        public virtual byte MaxY
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Indikuje zda byla zavolána metoda init.
        /// </summary>
        public bool IsInitiaized
        {
            get;
            private set;
        }

        /// <summary>
        /// Alokovat nebo vynulovat pole.
        /// </summary>
        private void AllocOrResetData()
        {
            int n_x, n_y;
            n_x = MaxX - 'A' + 1;
            n_y = MaxY;

            if (_data == null)
                _data = new Pieces[n_x, n_y];
            else
                Array.Clear(_data, 0, n_x * n_y);
        }

        /// <summary>
        /// inicializace desky = výchozí rozestavění figurek podle pravidel
        /// </summary>
        public void Init()
        {
            IsInitiaized = false;
            AllocOrResetData();
            IsInitiaized = true;

            try
            {
                OnInit();
            }
            catch (Exception exc)
            {
                IsInitiaized = false;
                throw exc;
            }
        }

        /// <summary>
        /// Místo pro implementaci inicializační metody.
        /// </summary>
        protected virtual void OnInit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Proved tah
        /// </summary>
        public void ApplyMove(Move move)
        {
            if (move == null)
                throw new ArgumentNullException("move");

            Coord src = move.Source;
            Coord tar = move.Target;

            this[src] = move.SourcePiece;
            this[tar] = move.TargetPiece;
        }

        /// <summary>
        /// Pro ověření souřadnice zadané uživatelem.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool IsCoordValid(Coord coord)
        {
            return (coord.x >= 'A' && coord.x <= MaxX) &&
                   (coord.y >= 1 && coord.y <= MaxY);
        }

        /// <summary>
        /// Ziska posunutou souradnici. Za okraj desky nedovoli jit a vraci null.
        /// Podporuje zatim pouze ortogonalni smery.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public Coord? GetRelativeCoord(Coord current, CoordDirectionEnum direction)
        {
            int deltaX = 0;
            int deltaY = 0;

            switch (direction)
            {
                case CoordDirectionEnum.deForward:
                    deltaY = 1;
                    break;
                case CoordDirectionEnum.deAft:
                    deltaY = -1;
                    break;
                case CoordDirectionEnum.deLeft:
                    deltaX = -1;
                    break;
                case CoordDirectionEnum.deRight:
                    deltaX = 1;
                    break;
            }

            if ((current.x + deltaX) > MaxX || (current.x + deltaX) < 'A')
                return null;
            else if ((current.y + deltaY) > MaxY || (current.y + deltaY) < 1)
                return null;
            else
                return Coord.Create((char)(current.x + deltaX), (byte)(current.y + deltaY));
        }
    }
}
