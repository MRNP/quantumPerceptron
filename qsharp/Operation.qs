namespace qsharp
{
    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Primitive;

operation Set (desired: Result, q1: Qubit) : ()
    {
        body
        {
            let current = M(q1);
            if (desired != current)
            {
                X(q1);
            }
        }
    }

    operation Grover () : (Int, Int)
    {
        body
        {
            mutable fmeasure = 0;
            mutable smeasure = 0;
      
            using(register = Qubit[2]){
            let first = register[0];
            let second = register[1];

            Set(Zero, first);
            Set(Zero, second);

            H(first);
            H(second);
            S(first);
            //S(second);

            H(second);
            CNOT(first, second);
            H(second);

            S(first);
            //S(second);
            H(first);
            H(second);
            
            X(first);
            X(second);
            H(second);
            CNOT(first, second);
            H(second);
            X(first);
            X(second);
            H(first);
            H(second);

            if(M(first) == One) {set fmeasure = 1;};
            if(M(second) == One) {set smeasure = 1;};

            // Reset all of the qubits that we used before releasing
                // them.
                ResetAll(register);
            }

            return (fmeasure, smeasure);

        }

    }
}
