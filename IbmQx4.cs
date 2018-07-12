// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace progettoreti
{
    /*
     * Quick and dirty driver to enable the IbmQx4
     */
    class IbmQx4 : QiskitDriver
    {
        public IbmQx4(string key) : base(key)
        {
        }

        public override int QBitCount => 5;

        public override string Name => "ibmqx4";
    }
}
