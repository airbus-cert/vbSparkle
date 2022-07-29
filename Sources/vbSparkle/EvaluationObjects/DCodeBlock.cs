using MathNet.Symbolics;

namespace vbSparkle
{
    public class DCodeBlock
        : DExpression
    {
        private string codeBlock;

        public override bool HasSideEffet {
            get => true;
            set => throw new System.NotImplementedException();
        }

        public DCodeBlock(string codeBlock)
        {
            this.codeBlock = codeBlock;
        }

        public override bool IsValuable
        {
            get
            {
                return false;
            }
            set => throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return this.codeBlock;
        }

        public override string ToExpressionString()
        {
            return ToString();
        }

        internal override SymbolicExpression GetSymExp()
        {
            return SymbolicExpression.Variable(ToString());
        }

        public override string ToValueString()
        {
            throw new System.NotImplementedException();
        }
    }
}