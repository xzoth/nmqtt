﻿/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// An Mqtt message that is used to initiate a connection to a message broker.
    /// </summary>
    public sealed partial class MqttConnectMessage : MqttMessage
    {
        /// <summary>
        /// Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttConnectVariableHeader VariableHeader { get; set; }

        /// <summary>
        /// Gets or sets the payload of the Mqtt Message.
        /// </summary>
        /// <value>The payload of the Mqtt Message.</value>
        public MqttConnectPayload Payload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectMessage"/> class.
        /// </summary>
        /// <remarks>
        /// Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttConnectMessage()
        {
            this.Header = new MqttHeader().AsType(MqttMessageType.Connect);

            this.VariableHeader = new MqttConnectVariableHeader()
            {
                ConnectFlags = new MqttConnectFlags()
            };

            this.Payload = new MqttConnectPayload(this.VariableHeader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectMessage"/> class.
        /// </summary>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttConnectMessage(MqttHeader header, Stream messageStream)
        {
            this.Header = header;
            ReadFrom(messageStream);
        }

        /// <summary>
        /// ss the message to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to write the message to.</param>
        public override void WriteTo(Stream messageStream)
        {
            this.Header.WriteTo(VariableHeader.GetWriteLength() + Payload.GetWriteLength(), messageStream);
            this.VariableHeader.WriteTo(messageStream);
            this.Payload.WriteTo(messageStream);
        }

        /// <summary>
        /// Reads a message from the supplied stream.
        /// </summary>
        /// <param name="messageStream">The message stream.</param>
        public override void ReadFrom(Stream messageStream)
        {
            this.VariableHeader = new MqttConnectVariableHeader(messageStream);
            this.Payload = new MqttConnectPayload(this.VariableHeader, messageStream);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(base.ToString());
            sb.AppendLine(VariableHeader.ToString());
            sb.AppendLine(Payload.ToString());

            return sb.ToString();
        }

    }
}
