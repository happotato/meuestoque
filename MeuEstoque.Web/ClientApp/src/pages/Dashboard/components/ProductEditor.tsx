import * as React from "react";
import { CreateProductDTO, Product } from "~/api";

export interface ProductEditorProps {
  defaultProduct?: Product;
  onSubmit?: (product: CreateProductDTO) => void;
}

export function ProductEditor(props: ProductEditorProps) {
  const [imageSrc, setImageSrc] = React.useState(props.defaultProduct?.imageUrl ?? "");

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);

    props.onSubmit?.({
      name: formData.get("product-name") as string,
      description: formData.get("desc") as string,
      imageUrl: formData.get("imageUrl") as string | undefined,
      price: parseFloat(formData.get("price") as string),
      quantity: parseInt(formData.get("quantity") as string, 10),
    });
  }

  return (
    <form className="container" onSubmit={onSubmit} autoComplete="off">
      <div className="mb-3 row">
        <div className="col-auto">
          <div
            className="bg-light border rounded overflow-hidden"
            style={{
              width: "10rem",
              height: "10rem",
              background: `url(https://via.placeholder.com/150)`,
              backgroundPosition: "center",
              backgroundSize: "contain",
            }}
          >
            {imageSrc.length > 0 &&
              <img className="w-100 h-100 object-fit-contain" src={imageSrc} />
            }
          </div>
        </div>
        <div className="col">
          <div className="mb-3">
            <label className="form-label">{"Product name"}</label>
            <input
              name="product-name"
              type="text"
              className="form-control"
              defaultValue={props.defaultProduct?.name}
              required
            />
          </div>
          <div>
            <label className="form-label">{"Image URL"}</label>
            <input
              name="imageUrl"
              type="url"
              className="form-control"
              value={imageSrc}
              onChange={(e) => setImageSrc(e.currentTarget.value)}
            />
          </div>
        </div>
      </div>
      <div className="mb-3">
        <label className="form-label">{"Description"}</label>
        <textarea
          name="desc"
          rows={3}
          className="form-control"
          defaultValue={props.defaultProduct?.description}
          required
        />
      </div>
      <div className="mb-3 row">
        <div className="col">
          <label className="form-label">{"Price"}</label>
          <div className="input-group">
            <label className="input-group-text text-muted">{"R$"}</label>
            <input
              name="price"
              type="number"
              className="form-control"
              step="0.1"
              min="0"
              defaultValue={props.defaultProduct?.price ?? 1}
              required
            />
          </div>
        </div>
        <div className="col">
          <label className="form-label">{"Quantity"}</label>
          <input
            name="quantity"
            type="number"
            className="form-control"
            defaultValue={props.defaultProduct?.quantity ?? 1}
            min="0"
            disabled={!!props.defaultProduct}
            required
          />
        </div>
      </div>
      <div className="mb-3">
        <button type="submit" className="btn btn-sm btn-primary">
          {"Save"}
        </button>
      </div>
    </form>
  );
}
