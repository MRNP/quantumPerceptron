namespace Qiskit
{
    /*
     * Quick and dirty driver to enable the IbmQx4
     */
    class IbmQx5 : QiskitDriver
    {
        public IbmQx5(string key) : base(key)
        {
        }

        public override int QBitCount => 5;

        public override string Name => "ibmqx5";
    }
}