import * as React from "react";
import { Order } from "~/api";

export interface OrderComponentProps {
  order: Order;
}

export function OrderComponent({ order }: OrderComponentProps) {
  return (
    <div
      className="card bg-light"
      style={{
        borderLeftStyle: "solid",
        borderLeftWidth: "5px",
        borderLeftColor:
          order.quantity >= 0 ? "var(--bs-green)" : "var(--bs-red)",
      }}
    >
      <div className="card-body lh-sm">
        <div className="row">
          <div className="col">
            <small className="text-muted">
              <b>{new Date(order.createdAt).toLocaleString()}</b>
              <b>{" - "}</b>
              <b>
                {order.quantity >= 0 && `${order.quantity} Incoming`}
                {order.quantity < 0 && `${Math.abs(order.quantity)} Outgoing`}
              </b>
              <b>{" - "}</b>
              <b>{`R$ ${order.price.toFixed(2).replace(".", ",")}`}</b>
            </small>
            <br />
            <span>{order.product.name}</span>
            <br />
            <small className="text-muted">{order.product.description}</small>
          </div>
        </div>
      </div>
    </div>
  );
}